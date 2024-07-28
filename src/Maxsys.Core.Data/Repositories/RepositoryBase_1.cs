using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Maxsys.Core.Data.Extensions;
using Maxsys.Core.Extensions;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.Core.Data;

/// <inheritdoc cref="IRepository{TEntity}"/>
public abstract class RepositoryBase<TEntity> : RepositoryBase, IRepository<TEntity>
    where TEntity : class
{
    protected readonly DbSet<TEntity> DbSet;
    protected readonly IMapper _mapper;

    #region CONSTRUCTOR

    public RepositoryBase(DbContext context, IMapper mapper)
        : base(context)
    {
        DbSet = Context.Set<TEntity>();
        _mapper = mapper;
    }

    #endregion CONSTRUCTOR

    #region PROT

    /// <remarks>
    /// <code>
    /// var query = @readonly ? DbSet.AsNoTracking() : DbSet.AsTracking();
    ///
    /// return ValueTask.FromResult(predicate is not null
    ///     ? query.Where(predicate)
    ///     : query);
    /// </code>
    /// </remarks>
    protected virtual ValueTask<IQueryable<TEntity>> GetQueryable(Expression<Func<TEntity, bool>>? predicate = null, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = @readonly ? DbSet.AsNoTracking() : DbSet.AsTracking();

        return ValueTask.FromResult(predicate is not null
            ? query.Where(predicate)
            : query);
    }

    protected virtual ValueTask<bool> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entry = DbSet.Remove(entity);

        return ValueTask.FromResult(entry.State == EntityState.Deleted);
    }

    protected Expression<Func<TEntity, bool>> GetIdExpression(object[] ids)
    {
        return DbSet.EntityType.GetIdExpression<TEntity>(ids);
    }

    #endregion PROT

    #region MOD

    public virtual async ValueTask<bool> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await DbSet.AddAsync(entity, cancellationToken);

        return entry.State == EntityState.Added;
    }

    public virtual async ValueTask<bool> AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var item in entities)
        {
            if (!await AddAsync(item, cancellationToken))
                return false;
        }

        return true;
    }

    public virtual async ValueTask<bool> DeleteAsync(object id, CancellationToken cancellationToken = default)
    {
        return await DeleteAsync(new object[] { id }, cancellationToken);
    }

    public virtual async ValueTask<bool> DeleteAsync(object[] id, CancellationToken cancellationToken = default)
    {
        var entity = await DbSet.FindAsync(id, cancellationToken: cancellationToken);

        return entity is null || await RemoveAsync(entity, cancellationToken);
    }

    public virtual async ValueTask<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entry = DbSet.Update(entity);

        return await ValueTask.FromResult(entry.State == EntityState.Modified);
    }

    public virtual async ValueTask<bool> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var item in entities)
        {
            if (!await UpdateAsync(item, cancellationToken))
                return false;
        }

        return true;
    }

    #endregion MOD

    #region UTIL

    public async ValueTask<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, true, cancellationToken);

        return await query.CountAsync(cancellationToken);
    }

    public async ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, true, cancellationToken);

        return await query.AnyAsync(cancellationToken);
    }

    public async ValueTask<bool> IdExistsAsync(object[] ids, CancellationToken cancellationToken = default)
    {
        var predicate = GetIdExpression(ids);

        return await AnyAsync(predicate, cancellationToken);
    }

    public bool HasChanges(TEntity entity, bool added = true, bool modified = true, bool deleted = true)
    {
        var state = Context.Entry(entity).State;
        return (added && state == EntityState.Added)
            || (modified && state == EntityState.Modified)
            || (deleted && state == EntityState.Deleted);
    }

    #endregion UTIL

    #region LIST

    public virtual async Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>>? predicate = null, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, @readonly, cancellationToken);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>>? predicate, ListCriteria criteria, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, @readonly, cancellationToken);

        return await query.ApplyCriteria(criteria).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>>? predicate, Pagination? pagination, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, @readonly, cancellationToken);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortSelector)
            : query.OrderByDescending(sortSelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, @readonly: true, cancellationToken);

        return await query.Select(projection).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, Expression<Func<TEntity, bool>>? predicate, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class
    {
        var query = await GetQueryable(predicate, @readonly: true, cancellationToken);

        return await query.Select(projection).ApplyCriteria(criteria).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, Expression<Func<TEntity, bool>>? predicate, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(predicate, @readonly: true, cancellationToken))
            .Select(projection);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortSelector)
            : query.OrderByDescending(sortSelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    // TODO testar retorno com struct
    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        CancellationToken cancellationToken = default) //where TDestination : class
    {
        var query = (await GetQueryable(predicate, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        ListCriteria criteria,
        CancellationToken cancellationToken = default) where TDestination : class
    {
        var query = (await GetQueryable(predicate, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
            .ApplyCriteria(criteria);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        Pagination? pagination,
        Expression<Func<TDestination, dynamic>> sortSelector,
        SortDirection sortDirection = SortDirection.Ascending,
        CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(predicate, false, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortSelector)
            : query.OrderByDescending(sortSelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    #endregion LIST

    #region GET

    public virtual Task<TDestination?> GetByIdAsync<TDestination>(object id, CancellationToken cancellationToken = default) //where TDestination : class
    {
        return GetByIdAsync<TDestination>([id], cancellationToken);
    }

    public virtual async Task<TDestination?> GetByIdAsync<TDestination>(object[] ids, CancellationToken cancellationToken = default) //where TDestination : class
    {
        var predicate = DbSet.EntityType.GetIdExpression<TEntity>(ids);
        var query = await GetQueryable(predicate, true, cancellationToken);

        return await query
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual Task<TEntity?> GetByIdAsync(object id, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        return GetByIdAsync([id], @readonly, cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(object[] ids, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var predicate = DbSet.EntityType.GetIdExpression<TEntity>(ids);
        var query = await GetQueryable(predicate, @readonly, cancellationToken);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, @readonly, cancellationToken);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, true, cancellationToken);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortSelector)
            : query.OrderByDescending(sortSelector);

        return await orderedQuery
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetWithIncludeAsync<TProperty>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProperty>> includeNavigation, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, @readonly, cancellationToken);

        return await query.Include(includeNavigation).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetWithIncludeAsync<TProperty>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProperty>> includeNavigation, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(predicate, @readonly, cancellationToken))
            .Include(includeNavigation);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortSelector)
            : query.OrderByDescending(sortSelector);

        return await orderedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        try
        {
            return await GetSingleOrThrowsAsync(predicate, @readonly, cancellationToken);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public virtual async Task<TEntity?> GetSingleOrThrowsAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, @readonly, cancellationToken);

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(predicate, true, cancellationToken))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default) where TDestination : class
    {
        var query = await GetQueryable(predicate, true, cancellationToken);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortSelector)
            : query.OrderByDescending(sortSelector);

        return await orderedQuery
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual Task<TDestination?> GetByIdAsync<TDestination>(object id, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        return GetByIdAsync([id], projection, cancellationToken);
    }

    public virtual async Task<TDestination?> GetByIdAsync<TDestination>(object[] ids, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var predicate = GetIdExpression(ids);
        var query = await GetQueryable(predicate, true, cancellationToken);

        return await query.Select(projection).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, true, cancellationToken);

        return await query.Select(projection).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, true, cancellationToken);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortSelector)
            : query.OrderByDescending(sortSelector);

        return await orderedQuery
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        try
        {
            return await GetSingleOrThrowsAsync<TDestination>(predicate, cancellationToken);
        }
        catch (Exception)
        {
            return default;
        }
    }

    public virtual async Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, true, cancellationToken);

        return await query.ProjectTo<TDestination>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken);
    }

    #endregion GET
}