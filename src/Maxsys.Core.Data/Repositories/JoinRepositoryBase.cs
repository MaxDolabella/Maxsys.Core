using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Maxsys.Core.Data.Extensions;
using Maxsys.Core.Extensions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.Core.Data;

/// <summary>
/// Repositório usado quando busca é baseada numa entidade <typeparamref name="TEntity"/>,
/// convertida em um objeto com join não natural <typeparamref name="TJoin"/> e enfim mapeada para o objeto de destino.
/// </summary>
public abstract class JoinRepositoryBase<TEntity, TJoin, TFilter> : RepositoryBase<TEntity>, IRepository<TEntity, TFilter>
    where TEntity : class
    where TJoin : class
    where TFilter : IFilter<TEntity>, new()
{
    #region CONSTRUCTOR

    public JoinRepositoryBase(DbContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    #endregion CONSTRUCTOR

    #region PROT

    protected IOrderedQueryable<T> ApplyOrderBy<T>(IQueryable<T> query, Expression<Func<T, dynamic>> sortSelector, SortDirection sortDirection)
    {
        return sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortSelector)
            : query.OrderByDescending(sortSelector);
    }

    /// <remarks>
    /// <code>
    /// var query = await GetQueryable(predicate: null, @readonly: true, cancellation);
    ///
    /// filters.SetExpressions();
    /// foreach (var expression in filters.Expressions)
    /// {
    ///     query = query.Where(expression);
    /// }
    /// </code>
    /// </remarks>
    protected virtual async ValueTask<IQueryable<TEntity>> GetQueryable(TFilter filters, bool @readonly = true, CancellationToken cancellation = default)
    {
        var query = await GetQueryable(predicate: null, @readonly: true, cancellation);

        filters.SetExpressions();
        foreach (var expression in filters.Expressions)
        {
            query = query.Where(expression);
        }

        return query;
    }

    /// <remarks>
    /// <code>
    /// var baseQuery = await GetQueryable(filters ?? new(), @readonly, cancellation);
    ///
    /// if(sortSelector is not null &amp;&amp; sortDirection is not null)
    /// {
    ///     baseQuery = ApplyOrderBy(baseQuery, sortSelector, sortDirection.Value);
    /// }
    ///
    /// var query = JoinConvert(baseQuery);
    /// </code>
    /// </remarks>
    protected virtual async ValueTask<IQueryable<TJoin>> GetJoinQueryable(TFilter? filters, Expression<Func<TEntity, dynamic>>? sortSelector = null, SortDirection? sortDirection = null, bool @readonly = true, CancellationToken cancellation = default)
    {
        var baseQuery = await GetQueryable(filters ?? new(), @readonly, cancellation);

        if (sortSelector is not null && sortDirection is not null)
        {
            baseQuery = ApplyOrderBy(baseQuery, sortSelector, sortDirection.Value);
        }

        var query = EntityToJoinQueryableConvert(baseQuery, filters);

        return await ValueTask.FromResult(@readonly ? query.AsNoTracking() : query.AsTracking());
    }

    /// <remarks>
    /// <code>
    /// var baseQuery = await GetQueryable(predicate, @readonly, cancellation);
    ///
    /// if(sortSelector is not null &amp;&amp; sortDirection is not null)
    /// {
    ///     baseQuery = ApplyOrderBy(baseQuery, sortSelector, sortDirection.Value);
    /// }
    ///
    /// var query = JoinConvert(baseQuery);
    /// </code>
    /// </remarks>
    protected virtual async ValueTask<IQueryable<TJoin>> GetJoinQueryable(Expression<Func<TEntity, bool>>? predicate, Expression<Func<TEntity, dynamic>>? sortSelector = null, SortDirection? sortDirection = null, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var baseQuery = await GetQueryable(predicate, @readonly, cancellationToken);

        if (sortSelector is not null && sortDirection is not null)
        {
            baseQuery = ApplyOrderBy(baseQuery, sortSelector, sortDirection.Value);
        }

        var query = EntityToJoinQueryableConvert(baseQuery, default(TFilter?));

        return await ValueTask.FromResult(@readonly ? query.AsNoTracking() : query.AsTracking());
    }

    /// <remarks>
    /// <code>
    /// return query.LeftOuterJoin(Context.OtherCollection,
    ///         entity => entity.otherId,
    ///         other => other.Id,
    ///         join => new { Entity = join.Outer, Other = join.Inner })
    ///     .Select(a => new Join
    ///     {
    ///         Entity = a.Entity,
    ///         Other = a.Other
    ///     });
    /// </code>
    /// </remarks>
    protected abstract IQueryable<TJoin> EntityToJoinQueryableConvert(IQueryable<TEntity> query, TFilter? filters);

    #endregion PROT

    #region QTY

    public virtual async ValueTask<int> CountAsync(TFilter filters, CancellationToken cancellation = default)
    {
        var query = await GetJoinQueryable(filters, null, null, true, cancellation);

        return await query.CountAsync(cancellation);
    }

    public virtual async ValueTask<bool> AnyAsync(TFilter filters, CancellationToken cancellation = default)
    {
        var query = await GetJoinQueryable(filters, null, null, true, cancellation);

        return await query.AnyAsync(cancellation);
    }

    #endregion QTY

    #region LIST

    public virtual async Task<List<TEntity>> ToListAsync(
        TFilter filters,
        bool @readonly = true,
        CancellationToken cancellationToken = default)
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
        Expression<Func<TEntity, dynamic>> sortSelector,
        SortDirection sortDirection = SortDirection.Ascending,
        bool @readonly = true,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly, cancellationToken);

        var orderedQuery = ApplyOrderBy(query, sortSelector, sortDirection);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    public override async Task<List<TDestination>> ToListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        CancellationToken cancellationToken = default)
    {
        var query = (await GetJoinQueryable(predicate, null, null, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.ToListAsync(cancellationToken);
    }

    public override async Task<List<TDestination>> ToListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        ListCriteria criteria,
        CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = (await GetJoinQueryable(predicate, null, null, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
            .ApplyCriteria(criteria);

        return await query.ToListAsync(cancellationToken);
    }

    public override async Task<List<TDestination>> ToListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        Pagination? pagination,
        Expression<Func<TDestination, dynamic>> sortSelector,
        SortDirection sortDirection = SortDirection.Ascending,
        CancellationToken cancellationToken = default)

    {
        var query = (await GetJoinQueryable(predicate, null, null, false, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        var orderedQuery = ApplyOrderBy(query, sortSelector, sortDirection);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    // ===

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        TFilter filters,
        CancellationToken cancellationToken = default)
    {
        var query = (await GetJoinQueryable(filters, null, null, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        TFilter filters,
        ListCriteria criteria,
        CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = (await GetJoinQueryable(filters, null, null, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
            .ApplyCriteria(criteria);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        TFilter filters,
        Pagination? pagination,
        Expression<Func<TDestination, dynamic>> sortSelector,
        SortDirection sortDirection = SortDirection.Ascending,
        CancellationToken cancellationToken = default)
    {
        var query = (await GetJoinQueryable(filters, null, null, false, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        var orderedQuery = ApplyOrderBy(query, sortSelector, sortDirection);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    #endregion LIST

    #region GET

    public virtual async Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
    {
        var query = (await GetJoinQueryable(filters, null, null, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public override async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TDestination : default
    {
        var query = (await GetJoinQueryable(predicate, null, null, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.FirstOrDefaultAsync(cancellationToken);
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

    public virtual async Task<TDestination?> GetAsync<TDestination>(TFilter filters, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var orderedQuery = await GetJoinQueryable(filters, sortSelector, sortDirection, true, cancellationToken);

        return await orderedQuery
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public override async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var orderedQuery = await GetJoinQueryable(predicate, sortSelector, sortDirection, true, cancellationToken);

        return await orderedQuery
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(TFilter filters, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly, cancellationToken);

        var orderedQuery = ApplyOrderBy(query, sortSelector, sortDirection);

        return await orderedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync<TProperty>(TFilter filters, Expression<Func<TEntity, TProperty>> includeNavigation, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(filters, @readonly, cancellationToken)).Include(includeNavigation);

        var orderedQuery = ApplyOrderBy(query, sortSelector, sortDirection);

        return await orderedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
    {
        var query = (await GetJoinQueryable(filters, null, null, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        try
        {
            return await query.SingleOrDefaultAsync(cancellationToken);
        }
        catch (Exception)
        {
            return default;
        }
    }

    public virtual async Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
    {
        var query = (await GetJoinQueryable(filters, null, null, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    public override Task<TDestination?> GetByIdAsync<TDestination>(object id, CancellationToken cancellationToken = default)
        where TDestination : default
    {
        return GetByIdAsync<TDestination>([id], cancellationToken);
    }

    public override async Task<TDestination?> GetByIdAsync<TDestination>(object[] ids, CancellationToken cancellationToken = default)
         where TDestination : default
    {
        var predicate = DbSet.EntityType.GetIdExpression<TEntity>(ids);
        var query = (await GetJoinQueryable(predicate, null, null, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    #endregion GET

    public Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        throw new NotImplementedException();
    }

    public Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TDestination?> GetAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}