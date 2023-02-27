using System.Collections.Generic;
using System.Linq.Expressions;
using Maxsys.ModelCore.Listing;
using Maxsys.ModelCore.Sorting;

namespace System.Linq;

public static partial class IQueryableExtensions
{
    /// <summary>
    /// Applies pahination to an IQueryable.
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
    /// Applies sorting to an IQueryable.
    /// </summary>
    public static IQueryable<T> ApplySort<T>(
        this IQueryable<T> query,
        IEnumerable<SortFilter>? sortFilters,
        Func<byte?, Expression<Func<T, dynamic>>> sortSelectorFunc)
      where T : class
    {
        if (sortFilters is null || !sortFilters.Any())
        {
            sortFilters = new List<SortFilter> { SortFilter.DEFAULT_SORT };
        }

        IOrderedQueryable<T>? orderedQuery = null;

        foreach (var sort in sortFilters)
        {
            var keySelector = sortSelectorFunc(sort.Column);

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
    /// Applies sorting to an IQueryable.
    /// </summary>
    public static IQueryable<T> ApplySort<T>(
        this IQueryable<T> query,
        IEnumerable<SortFilter>? sortFilters,
        ISortColumnSelector<T> sortSelector)
      where T : class
    {
        return ApplySort(query, sortFilters, sortSelector.ColumnSelector);
    }

    /// <summary>
    /// Shortcut to <c>query.GroupJoin(...).SelectMany(...)</c>.
    /// <example>
    /// <code>
    /// LeftOuterJoin(context.Set&lt;Location&gt;(),
    /// source => source.InnerId, inner => inner.Id,
    /// join => new { source = join.Outer, inner = join.Inner })
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

    public class LeftOuterJoinResult<TSource, TInner>
    {
        public TSource Outer { get; set; }
        public TInner? Inner { get; set; }
    }
}