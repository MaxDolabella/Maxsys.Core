using System.Linq.Expressions;
using Maxsys.Core;
using Maxsys.Core.Extensions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Mapping;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Sorting;
using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Maxsys.Data;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <inheritdoc cref="IRepository{TEntity, TFilter}"/>
public class RepositoryBase<TEntity, TFilter> : RepositoryBase<TEntity>, IRepository<TEntity, TFilter>
    where TEntity : class
    where TFilter : IFilter<TEntity>
{
    #region CONSTRUCTOR

    public RepositoryBase(DbContext context, IQueryProjector projector)
        : base(context, projector)
    { }

    #endregion CONSTRUCTOR

    #region PROT

    /// <remarks>
    /// <code>
    /// var query = await GetQueryable(predicate: null, @readonly, cancellation);
    ///
    /// filters.ApplyFilter(ref query);
    ///
    /// return query;
    /// </code>
    /// </remarks>
    protected virtual async ValueTask<IQueryable<TEntity>> GetQueryable(TFilter filters, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await base.GetQueryable(predicate: null, @readonly, cancellationToken);

        filters.ApplyFilter(ref query);

        return query;
    }

    #endregion PROT

    #region QTY

    public virtual async ValueTask<int> CountAsync(TFilter filters, CancellationToken cancellation = default)
    {
        var query = await GetQueryable(filters, true, cancellation);

        return await query.CountAsync(cancellation);
    }

    public virtual async ValueTask<int> CountAsync<TDestination>(TFilter filters, ListCriteria criteria, CancellationToken cancellation = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(filters, true, cancellation))
            .ApplyFilters(criteria.Filters)
            .ApplySearch(criteria.Search);

        return await query.CountAsync(cancellation);
    }

    public virtual async ValueTask<bool> AnyAsync(TFilter filters, CancellationToken cancellation = default)
    {
        var query = await GetQueryable(filters, true, cancellation);

        return await query.AnyAsync(cancellation);
    }

    #endregion QTY

    #region LIST

    public virtual async Task<List<TEntity>> ToListAsync(TFilter filters, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly, cancellationToken);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> ToListAsync(
        TFilter filters,
        ListCriteria criteria,
        bool @readonly = true,
        CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(filters, @readonly, cancellationToken))
            .ApplyCriteria(criteria);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> ToListAsync(
        TFilter filters,
        Pagination? pagination,
        Expression<Func<TEntity, dynamic>> sortKeySelector,
        SortDirection sortDirection = SortDirection.Ascending,
        bool @readonly = true,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly, cancellationToken);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKeySelector)
            : query.OrderByDescending(sortKeySelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    // ===

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        TFilter filters,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(filters, true, cancellationToken));

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        TFilter filters,
        ListCriteria criteria,
        CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(filters, true, cancellationToken))
            .ApplyCriteria(criteria);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        TFilter filters,
        Pagination? pagination,
        Expression<Func<TDestination, dynamic>> sortKeySelector,
        SortDirection sortDirection = SortDirection.Ascending,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(filters, false, cancellationToken));

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKeySelector)
            : query.OrderByDescending(sortKeySelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly: true, cancellationToken);

        return await ApplyProjection(query, projection).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = await GetQueryable(filters, @readonly: true, cancellationToken);

        return await ApplyProjection(query, projection).ApplyCriteria(criteria).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var query = ApplyProjection(await GetQueryable(filters, @readonly: true, cancellationToken), projection);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKeySelector)
            : query.OrderByDescending(sortKeySelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    #endregion LIST

    #region GET

    public virtual async Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(filters, true, cancellationToken));

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, true, cancellationToken);

        return await ApplyProjection(query, projection).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(TFilter filters, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly, cancellationToken);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync<TProperty>(TFilter filters, Expression<Func<TEntity, TProperty>> includeNavigation, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly, cancellationToken);

        return await query.Include(includeNavigation).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(TFilter filters, Expression<Func<TEntity, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, true, cancellationToken);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKeySelector)
            : query.OrderByDescending(sortKeySelector);

        return await ApplyProjection<TDestination>(orderedQuery)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(TFilter filters, Expression<Func<TEntity, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly, cancellationToken);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKeySelector)
            : query.OrderByDescending(sortKeySelector);

        return await orderedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync<TProperty>(TFilter filters, Expression<Func<TEntity, TProperty>> includeNavigation, Expression<Func<TEntity, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(filters, @readonly, cancellationToken)).Include(includeNavigation);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKeySelector)
            : query.OrderByDescending(sortKeySelector);

        return await orderedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        try
        {
            return await GetSingleOrThrowsAsync<TDestination>(filters, cancellationToken);
        }
        catch (Exception)
        {
            return default;
        }
    }

    public virtual async Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(filters, true, cancellationToken));

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    #endregion GET
}