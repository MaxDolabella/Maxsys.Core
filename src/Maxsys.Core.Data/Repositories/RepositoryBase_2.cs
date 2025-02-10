using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Maxsys.Core.Extensions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.Core.Data;

/// <inheritdoc cref="IRepository{TEntity, TFilter}"/>
public abstract class RepositoryBase<TEntity, TFilter> : RepositoryBase<TEntity>, IRepository<TEntity, TFilter>
    where TEntity : class
    where TFilter : IFilter<TEntity>
{
    #region CONSTRUCTOR

    public RepositoryBase(DbContext context, IMapper mapper)
        : base(context, mapper)
    { }

    #endregion CONSTRUCTOR

    #region PROT

    /// <remarks>
    /// <code>
    /// var query = await GetQueryable(predicate: null, @readonly: true, cancellation);
    ///
    /// filters.SetExpressions();
    /// foreach (var expression in filters.Expressions)
    ///     query = query.Where(expression);
    ///
    /// return query;
    /// </code>
    /// </remarks>
    protected virtual async ValueTask<IQueryable<TEntity>> GetQueryable(TFilter filters, bool @readonly = true, CancellationToken cancellation = default)
    {
        var query = await GetQueryable(predicate: null, @readonly: true, cancellation);

        filters.ApplyFilter(query);

        return query;
    }

    #endregion PROT

    #region QTY

    public virtual async ValueTask<int> CountAsync(TFilter filters, CancellationToken cancellation = default)
    {
        var query = await GetQueryable(filters, true, cancellation);

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
        var query = (await GetQueryable(filters, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        TFilter filters,
        ListCriteria criteria,
        CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = (await GetQueryable(filters, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
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
        var query = (await GetQueryable(filters, false, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKeySelector)
            : query.OrderByDescending(sortKeySelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly: true, cancellationToken);

        return await query.Select(projection).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = await GetQueryable(filters, @readonly: true, cancellationToken);

        return await query.Select(projection).ApplyCriteria(criteria).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(filters, @readonly: true, cancellationToken))
            .Select(projection);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKeySelector)
            : query.OrderByDescending(sortKeySelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    #endregion LIST

    #region GET

    public virtual async Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(filters, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, true, cancellationToken);

        return await query.Select(projection).FirstOrDefaultAsync(cancellationToken);
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

        return await orderedQuery
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
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
    {
        var query = (await GetQueryable(filters, true, cancellationToken))
           .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    #endregion GET
}