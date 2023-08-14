using System.Linq.Expressions;
using Maxsys.Core;
using Maxsys.Core.Sorting;

namespace System.Linq;

/// <summary>
/// Fornece métodos de extensão para IQueryables
/// </summary>
public static class IQueryableExtensions
{
    /// <summary>
    /// Atalho para <c>query.ApplySort().ApplyPagination()</c>
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="criteria"></param>
    /// <param name="sortSelector"></param>
    /// <returns></returns>
    public static IQueryable<TSource> ApplyCriteria<TSource>(
        this IQueryable<TSource> source,
        ListCriteria criteria,
        ISortColumnSelector<TSource> sortSelector)
        where TSource : class
    {
        return source.ApplySort(criteria.Sorts, sortSelector)
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
        where TSource : class
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
    public static IQueryable<T> ApplySort<T>(
        this IQueryable<T> query,
        IEnumerable<SortFilter>? sortFilters,
        ISortColumnSelector<T> sortSelector)
      where T : class
    {
        if (sortFilters is null || !sortFilters.Any())
        {
            /* Aqui é possível definir ordenação padrão para cada tipo individualmente (caso nenhuma ordenação for passada).
            // Por exemplo, se a ordenação default de FollowUp for Date DESC, então:
            if (typeof(T) == typeof(FollowUp))
            {
                sortFilters = new List<SortFilter> { new((byte)ProcessFollowUpSortableColumns.Date, SortDirection.Descendant) };
            }
            else
            {
                sortFilters = new List<SortFilter> { new() };
            }
            //*/

            sortFilters = new List<SortFilter> { new() };
        }

        IOrderedQueryable<T>? orderedQuery = null;

        foreach (var sort in sortFilters)
        {
            var keySelector = sortSelector.ColumnSelector(sort.Column);

            orderedQuery = (keySelector, sort.Direction) switch
            {
                (not null, SortDirection.Ascendant) => orderedQuery is null ? query.OrderBy(keySelector) : orderedQuery.ThenBy(keySelector),
                (not null, SortDirection.Descendant) => orderedQuery is null ? query.OrderByDescending(keySelector) : orderedQuery.ThenByDescending(keySelector),
                _ => orderedQuery
            };
        }

        return orderedQuery ?? query;
    }

    /// <summary>
    /// Atalho para query.GroupJoin(...).SelectMany(...).Select(...)
    /// <example>
    /// <code>
    /// locations.LeftOuterJoin(countries,
    /// location => location.CountryId,
    /// country => country.Id,
    /// join => new { Location = join.Outer, Country = join.Inner }) //Country:null
    /// </code>
    /// </example>
    /// </summary>
    /// <returns></returns>
    public static IQueryable<TResult> LeftOuterJoin<TSource, TInner, TKey, TResult>(
        this IQueryable<TSource> outer,
        IQueryable<TInner> inner,
        Expression<Func<TSource, TKey>> outerKeySelector,
        Expression<Func<TInner, TKey>> innerKeySelector,
        Expression<Func<LeftOuterJoinResult<TSource, TInner?>, TResult>> resultSelector)
    {
        return outer.GroupJoin(inner,
          outerKeySelector,
          innerKeySelector,
          (outer, innerList) => new
          {
              Outer = outer,
              InnerList = innerList
          })
            .SelectMany(a => a.InnerList.DefaultIfEmpty(),
                (a, innerItem) => new LeftOuterJoinResult<TSource, TInner?> { Outer = a.Outer, Inner = innerItem })
            .Select(resultSelector);
    }

    /// <summary>
    /// Atalho para query.GroupJoin(...).Select(...)
    /// <example>
    /// <code>
    /// countries.LeftOuterJoin(location,
    /// country => country.Id,
    /// location => location.CountryId,
    /// join => new { Country = join.Outer, Locations = join.InnerList }) //IEnumerable&lt;Location&gt;
    /// </code>
    /// </example>
    /// </summary>
    /// <returns></returns>
    public static IQueryable<TResult> LeftOuterJoin<TSource, TInner, TKey, TResult>(
        this IQueryable<TSource> outer,
        IQueryable<TInner> inner,
        Expression<Func<TSource, TKey>> outerKeySelector,
        Expression<Func<TInner, TKey>> innerKeySelector,
        Expression<Func<LeftOuterJoinListResult<TSource, TInner>, TResult>> resultSelector)
    {
        return outer.GroupJoin(inner,
            outerKeySelector,
            innerKeySelector,
            (outer, innerList) => new LeftOuterJoinListResult<TSource, TInner>
            {
                Outer = outer,
                InnerList = innerList
            })
            .Select(resultSelector);
    }

    /// <summary>
    /// Classe auxiliar para realização de Left Outer Join
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TInner"></typeparam>
    public class LeftOuterJoinResult<TSource, TInner>
    {
        public TSource Outer { get; set; }
        public TInner? Inner { get; set; }
    }

    /// <summary>
    /// Classe auxiliar para realização de Left Outer Join
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TInner"></typeparam>
    public class LeftOuterJoinListResult<TSource, TInner>
    {
        public TSource Outer { get; set; }
        public IEnumerable<TInner> InnerList { get; set; }
    }
}