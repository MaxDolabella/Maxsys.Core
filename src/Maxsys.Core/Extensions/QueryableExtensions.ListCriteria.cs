using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Maxsys.Core.Filtering;
using Maxsys.Core.Helpers;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Extensions;

/// <summary>
/// Fornece métodos de extensão para IQueryables
/// </summary>
public static partial class QueryableExtensions
{
    extension<TSource>(IQueryable<TSource> source) where TSource : class
    {
        /// <summary>
        /// Atalho para <c>query.ApplySort().ApplyPagination()</c>
        /// </summary>
        public IQueryable<TSource> ApplyCriteria(ListCriteria criteria)
        {
            return source
                .ApplyFilters(criteria.Filters)
                .ApplySearch(criteria.Search)
                .ApplySort(criteria.Sorts)
                .ApplyPagination(criteria.Pagination);
        }

        /// <summary>
        /// Aplica filtros de coluna a um IQueryable, construindo dinamicamente as expressões de filtro.
        /// </summary>
        /// <typeparam name="TSource">O tipo da entidade a ser filtrada.</typeparam>
        /// <param name="filters">Array de filtros de coluna a serem aplicados. Cada filtro especifica um campo,
        /// um valor e um modo de comparação (ex: Contains, Equals, StartsWith).</param>
        /// <returns>
        /// Um <see cref="IQueryable{TSource}"/> com os filtros aplicados. Se <paramref name="filters"/> for nulo,
        /// vazio, ou se todos os filtros tiverem valores nulos, retorna o <paramref name="source"/> original.
        /// </returns>
        /// <remarks>
        /// Este método itera sobre cada filtro fornecido e:
        /// <list type="bullet">
        /// <item>Ignora filtros com <see cref="ColumnFilter.Value"/> nulo.</item>
        /// <item>Usa <see cref="ExpressionHelper.BuildColumnFilterExpression{T}"/> para construir dinamicamente
        /// as expressões de predicado baseadas no campo e modo de comparação.</item>
        /// <item>Aplica cada expressão válida usando <see cref="Queryable.Where{TSource}(IQueryable{TSource}, System.Linq.Expressions.Expression{Func{TSource, bool}})"/>.</item>
        /// </list>
        /// Os filtros são aplicados sequencialmente (operação AND), permitindo refinar progressivamente os resultados.
        /// </remarks>
        public IQueryable<TSource> ApplyFilters(ICollection<ColumnFilter> filters)
        {
            if (filters is null || filters.Count == 0)
                return source;

            var queryable = source;

            foreach (var filter in filters)
            {
                if (filter.Value is null)
                    continue;

                var expression = ExpressionHelper.BuildColumnFilterExpression<TSource>(filter);
                if (expression is not null)
                    queryable = queryable.Where(expression);
            }

            return queryable;
        }

        /// <summary>
        /// Aplica busca textual global em um <see cref="IQueryable{TSource}"/>, filtrando por propriedades
        /// decoradas com <see cref="SearchableAttribute"/>.
        /// </summary>
        /// <param name="search">O termo de busca. Se nulo ou vazio, retorna a query inalterada.</param>
        /// <returns>Um <see cref="IQueryable{TSource}"/> com o filtro de busca aplicado via OR entre as propriedades searchable.</returns>
        public IQueryable<TSource> ApplySearch(string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return source;

            var paths = _searchablePathsCache.GetOrAdd(typeof(TSource), GetSearchablePaths);

            if (paths.Length == 0)
                return source;

            var parameter = Expression.Parameter(typeof(TSource), "x");
            var searchConstant = Expression.Constant(search, typeof(string));
            var containsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;

            Expression? combined = null;

            foreach (var path in paths)
            {
                Expression memberAccess = parameter;
                foreach (var segment in path.Split('.'))
                {
                    var propInfo = memberAccess.Type.GetProperty(segment, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (propInfo is null)
                    {
                        memberAccess = null!;
                        break;
                    }
                    memberAccess = Expression.Property(memberAccess, propInfo);
                }

                if (memberAccess is null or ParameterExpression)
                    continue;

                var notNull = Expression.NotEqual(memberAccess, Expression.Constant(null, typeof(string)));
                var containsCall = Expression.Call(memberAccess, containsMethod, searchConstant);
                var safe = Expression.AndAlso(notNull, containsCall);

                combined = combined is null ? safe : Expression.OrElse(combined, safe);
            }

            if (combined is null)
                return source;

            var lambda = Expression.Lambda<Func<TSource, bool>>(combined, parameter);
            return source.Where(lambda);
        }

        /// <summary>
        /// IQueryableExtensions.<br/>
        /// Aplica ordenação em uma query.
        /// </summary>
        public IQueryable<TSource> ApplySort(List<SortFilter>? sortFilters)
        {
            if (!(sortFilters?.Count > 0))
            {
                if (!TryGetDefaultSort<TSource>(out SortFilter? defaultSort))
                    return source;

                sortFilters = [defaultSort];
            }

            if (sortFilters.Any(s => string.IsNullOrWhiteSpace(s.Field)))
            {
                throw new InvalidOperationException("Ordenação por coluna enum (byte) foi removida. Utilize SortFilter baseado em Field (string).");
            }

            return ApplySortString(source, sortFilters);
        }
    }

    extension<TSource>(IQueryable<TSource> source)
    {
        /// <summary>
        /// Aplica paginação em um IQueryable.
        /// </summary>
        public IQueryable<TSource> ApplyPagination(Pagination? pagination)
        {
            return pagination?.IsNotEmpty() == true
                ? source
                    .Skip(pagination.Size * pagination.Index)
                    .Take(pagination.Size)
                : source;
        }
    }

    #region Search (private)

    private static readonly ConcurrentDictionary<Type, string[]> _searchablePathsCache = new();

    /// <summary>
    /// Descobre os full paths das propriedades searchable para um tipo.
    /// </summary>
    private static string[] GetSearchablePaths(Type type)
    {
        var paths = new List<string>();

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var attributes = prop.GetCustomAttributes<SearchableAttribute>(inherit: true).ToArray();
            if (attributes.Length == 0)
                continue;

            foreach (var attr in attributes)
            {
                if (attr.Path is null)
                {
                    if (prop.PropertyType == typeof(string))
                        paths.Add(prop.Name);
                }
                else
                {
                    paths.Add($"{prop.Name}.{attr.Path}");
                }
            }
        }

        return [.. paths];
    }

    #endregion Search (private)

    #region Sort (private)

    private static bool TryGetDefaultSort<T>([NotNullWhen(true)] out SortFilter? defaultSort)
    {
        if (typeof(T).TryGetAttribute(out DefaultSortAttribute? defaultSortAttribute))
        {
            defaultSort = new SortFilter(defaultSortAttribute.Property, defaultSortAttribute.SortDirection);
        }
        else
        {
            defaultSort = null;
        }

        return defaultSort is not null;
    }

    private static IQueryable<T> ApplySortString<T>(IQueryable<T> query, List<SortFilter> sortFilters)
        where T : class
    {
        IOrderedQueryable<T>? orderedQuery = null;

        foreach (var sort in sortFilters)
        {
            var keySelector = ExpressionHelper.GetMemberAccessExpression<T>(sort.Field!);

            orderedQuery = (keySelector, sort.Direction) switch
            {
                (not null, SortDirection.Ascending) => orderedQuery is null ? query.OrderBy(keySelector) : orderedQuery.ThenBy(keySelector),
                (not null, SortDirection.Descending) => orderedQuery is null ? query.OrderByDescending(keySelector) : orderedQuery.ThenByDescending(keySelector),
                _ => orderedQuery
            };
        }

        return orderedQuery ?? query;
    }

    #endregion Sort (private)
}