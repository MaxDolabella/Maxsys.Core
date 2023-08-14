using System.Linq.Expressions;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.Core.Data;

/// <inheritdoc cref="IRepository{TEntity}"/>
public abstract class RepositoryBase<TEntity> : RepositoryBase, IRepository<TEntity>
    where TEntity : class
{
    protected readonly DbSet<TEntity> DbSet;

    #region CONSTRUCTOR

    public RepositoryBase(DbContext context)
        : base(context)
    {
        DbSet = Context.Set<TEntity>();
    }

    #endregion CONSTRUCTOR

    #region PRIV

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

    #endregion PRIV

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

    protected virtual async ValueTask<bool> RemoveAsync(TEntity obj, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entry = DbSet.Remove(obj);

        return await Task.FromResult(entry.State == EntityState.Deleted);
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

    #region QTY

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

    #endregion QTY

    #region LIST

    public virtual async Task<IReadOnlyList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, @readonly, cancellationToken);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate, ListCriteria criteria, ISortColumnSelector<TEntity> sortColumnSelector, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, @readonly, cancellationToken);

        return await query.ApplyCriteria(criteria, sortColumnSelector).ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate, Pagination? pagination, Expression<Func<TEntity, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascendant, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, @readonly, cancellationToken);

        var orderedQuery = sortDirection == SortDirection.Ascendant
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    #endregion LIST

    #region GET

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, @readonly, cancellationToken);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync<TProperty>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProperty>> includeNavigation, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, @readonly, cancellationToken);

        return await query.Include(includeNavigation).FirstOrDefaultAsync(cancellationToken);
    }

    #endregion GET
}