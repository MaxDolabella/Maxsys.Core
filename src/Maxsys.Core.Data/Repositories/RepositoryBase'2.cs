using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    protected readonly IMapper _mapper;

    #region CONSTRUCTOR

    public RepositoryBase(DbContext context, IMapper mapper)
        : base(context)
    {
        _mapper = mapper;
    }

    #endregion CONSTRUCTOR

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

        cancellation.ThrowIfCancellationRequested();

        filters.SetExpressions();
        foreach (var expression in filters.Expressions)
        {
            query = query.Where(expression);
        }

        return query;
    }

    #region Quant

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

    #endregion Quant

    #region LIST

    public virtual async Task<IReadOnlyList<TEntity>> GetListAsync(
        TFilter filters,
        bool @readonly = true,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly, cancellationToken);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetListAsync(
        TFilter filters,
        ListCriteria criteria,
        ISortColumnSelector<TEntity> sortColumnSelector,
        bool @readonly = true,
        CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(filters, @readonly, cancellationToken))
            .ApplyCriteria(criteria, sortColumnSelector);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetListAsync(
        TFilter filters,
        Pagination? pagination,
        Expression<Func<TEntity, dynamic>> keySelector,
        SortDirection sortDirection = SortDirection.Ascendant,
        bool @readonly = true,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly, cancellationToken);

        var orderedQuery = sortDirection == SortDirection.Ascendant
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TDestination>> GetListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        CancellationToken cancellationToken = default) where TDestination : class
    {
        var query = (await GetQueryable(predicate, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TDestination>> GetListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        ListCriteria criteria,
        ISortColumnSelector<TDestination> sortColumnSelector,
        CancellationToken cancellationToken = default) where TDestination : class
    {
        var query = (await GetQueryable(predicate, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
            .ApplyCriteria(criteria, sortColumnSelector);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TDestination>> GetListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        Pagination? pagination,
        Expression<Func<TDestination, dynamic>> keySelector,
        SortDirection sortDirection = SortDirection.Ascendant,
        CancellationToken cancellationToken = default) where TDestination : class
    {
        var query = (await GetQueryable(predicate, false, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        var orderedQuery = sortDirection == SortDirection.Ascendant
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    // ===

    public virtual async Task<IReadOnlyList<TDestination>> GetListAsync<TDestination>(
        TFilter filters,
        CancellationToken cancellationToken = default) where TDestination : class
    {
        var query = (await GetQueryable(filters, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TDestination>> GetListAsync<TDestination>(
        TFilter filters,
        ListCriteria criteria,
        ISortColumnSelector<TDestination> sortColumnSelector,
        CancellationToken cancellationToken = default) where TDestination : class
    {
        var query = (await GetQueryable(filters, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
            .ApplyCriteria(criteria, sortColumnSelector);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TDestination>> GetListAsync<TDestination>(
        TFilter filters,
        Pagination? pagination,
        Expression<Func<TDestination, dynamic>> keySelector,
        SortDirection sortDirection = SortDirection.Ascendant,
        CancellationToken cancellationToken = default) where TDestination : class
    {
        var query = (await GetQueryable(filters, false, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        var orderedQuery = sortDirection == SortDirection.Ascendant
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    #endregion LIST

    #region GET

    public virtual async Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class
    {
        var query = (await GetQueryable(filters, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TDestination : class
    {
        var query = (await GetQueryable(predicate, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(TFilter filters, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(filters, @readonly, cancellationToken));

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync<TProperty>(TFilter filters, Expression<Func<TEntity, TProperty>> includeNavigation, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(filters, @readonly, cancellationToken));

        return await query.Include(includeNavigation).FirstOrDefaultAsync(cancellationToken);
    }

    #endregion GET
}