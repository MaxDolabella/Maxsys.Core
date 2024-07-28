using Maxsys.Core.Helpers;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Extensions;

/// <summary>
/// Fornece métodos de extensão para IQueryables
/// </summary>
public static partial class QueryableExtensions
{
    /// <summary>
    /// Atalho para <c>query.ApplySort().ApplyPagination()</c>
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="criteria"></param>
    /// <returns></returns>
    public static IQueryable<TSource> ApplyCriteria<TSource>(
        this IQueryable<TSource> source,
        ListCriteria criteria)
        where TSource : class
    {
        return source
            .ApplySort(criteria.Sorts)
            .ApplyPagination(criteria.Pagination);
    }

    /// <summary>
    /// Aplica paginação em um IQueryable.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    public static IQueryable<TSource> ApplyPagination<TSource>(
        this IQueryable<TSource> source, Pagination? pagination)
    {
        if (pagination?.IsNotEmpty() == true)
            source = source
                .Skip(pagination.Size * pagination.Index)
                .Take(pagination.Size);

        return source;
    }

    /// <summary>
    /// IQueryableExtensions.<br/>
    /// Aplica ordenação em uma query.
    /// </summary>
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, List<SortFilter>? sortFilters)
        where T : class
    {
        // Default Sort
        if (!(sortFilters?.Count > 0))
        {
            if (!TryGetDefaultSort<T>(out SortFilter? defaultSort))
                return query;

            sortFilters = [defaultSort!];
        }

        // Verifica consistência dos sorts.
        // Todos precisam ser de um tipo
        var isAllString = sortFilters.All(s => !string.IsNullOrWhiteSpace(s.ColumnName));
        var isAllBytes = sortFilters.All(s => string.IsNullOrWhiteSpace(s.ColumnName));

        if (isAllString == isAllBytes) // true/false ou false/true ou false/false (nunca será true/true)
        {
            throw new InvalidOperationException("Mixed sorts are not acceptable. All sorts must be exclusively bytes or string.");
        }

        return isAllString
            ? ApplySortString(query, sortFilters)
            : ApplySortEnum(query, sortFilters);
    }

    private static IQueryable<T> ApplySortEnum<T>(this IQueryable<T> query, List<SortFilter> sortFilters)
        where T : class
    {
        // Verifica se o obj T tem um atributo SortableAttribute que contém
        // o enum que define suas properties ordenáveis.
        if (!typeof(T).TryGetAttribute(out SortableAttribute? sortableAttribute))
            return query;

        IOrderedQueryable<T>? orderedQuery = null;

        foreach (var sort in sortFilters)
        {
            if (!Enum.IsDefined(sortableAttribute!.SortColumnsType, sort.Column))
                continue;

            // Obtém-se o literal de acordo com o Enum especificado em SortableAttribute
            var literal = (Enum)Enum.ToObject(sortableAttribute!.SortColumnsType, sort.Column);
            var columnName = literal.ToSortablePropertyName()!;

            var keySelector = ExpressionHelper.GetMemberAccessExpression<T>(columnName);

            orderedQuery = (keySelector, sort.Direction) switch
            {
                (not null, SortDirection.Ascending) => orderedQuery is null ? query.OrderBy(keySelector) : orderedQuery.ThenBy(keySelector),
                (not null, SortDirection.Descending) => orderedQuery is null ? query.OrderByDescending(keySelector) : orderedQuery.ThenByDescending(keySelector),
                _ => orderedQuery
            };
        }

        return orderedQuery ?? query;
    }

    private static IQueryable<T> ApplySortString<T>(this IQueryable<T> query, List<SortFilter> sortFilters)
        where T : class
    {
        IOrderedQueryable<T>? orderedQuery = null;

        foreach (var sort in sortFilters)
        {
            var keySelector = ExpressionHelper.GetMemberAccessExpression<T>(sort.ColumnName!);

            orderedQuery = (keySelector, sort.Direction) switch
            {
                (not null, SortDirection.Ascending) => orderedQuery is null ? query.OrderBy(keySelector) : orderedQuery.ThenBy(keySelector),
                (not null, SortDirection.Descending) => orderedQuery is null ? query.OrderByDescending(keySelector) : orderedQuery.ThenByDescending(keySelector),
                _ => orderedQuery
            };
        }

        return orderedQuery ?? query;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="defaultSort"></param>
    /// <returns></returns>
    private static bool TryGetDefaultSort<T>(out SortFilter? defaultSort)
    {
        if (typeof(T).TryGetAttribute(out SortableAttribute? sortableAttribute))
        {
            var minEnum = EnumExtensions.Min<byte>(sortableAttribute.SortColumnsType);
            defaultSort = new(minEnum, SortDirection.Ascending);
        }
        else if (typeof(T).TryGetAttribute(out DefaultSortAttribute? defaultSortAttribute))
        {
            defaultSort = new SortFilter(defaultSortAttribute!.Property, SortDirection.Ascending);
        }
        else
        {
            defaultSort = null; ;
        }

        return defaultSort is not null;
    }
}