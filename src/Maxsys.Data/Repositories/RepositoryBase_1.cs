using System.Linq.Expressions;
using Maxsys.Data.Extensions;
using Maxsys.Core.Extensions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Mapping;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.Data;

/// <inheritdoc cref="IRepository{TEntity}"/>
public class RepositoryBase<TEntity> : RepositoryBase, IRepository<TEntity>
    where TEntity : class
{
    protected readonly DbSet<TEntity> DbSet;
    protected readonly IQueryProjector _projector;

    #region CONSTRUCTOR

    public RepositoryBase(DbContext context, IQueryProjector projector)
        : base(context)
    {
        DbSet = Context.Set<TEntity>();
        _projector = projector;
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

    /// <summary>
    /// Chokepoint único para projeção <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/>
    /// via <see cref="IQueryProjector"/>. Subclasses podem sobrescrever para injetar políticas de leitura
    /// (ex.: Field-Level Security) que reescrevam o <c>Select</c> traduzido para SQL.
    /// </summary>
    /// <remarks>
    /// Implementação default: <c>_projector.Project&lt;TDestination&gt;(source)</c>.
    /// Toda projeção interna baseada em mapeador passa por aqui — não chame
    /// <see cref="IQueryProjector.Project{TDestination}"/> diretamente em <see cref="RepositoryBase{TEntity}"/>.
    /// </remarks>
    protected virtual IQueryable<TDestination> ApplyProjection<TDestination>(IQueryable<TEntity> source)
        => _projector.Project<TDestination>(source);

    /// <summary>
    /// Chokepoint único para projeção <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/>
    /// via expressão fornecida pelo caller. Subclasses podem sobrescrever para reescrever a
    /// expressão antes da tradução para SQL (ex.: remover acesso a campos sensíveis).
    /// </summary>
    /// <remarks>
    /// Implementação default: <c>source.Select(projection)</c>.
    /// Toda projeção interna baseada em <c>Expression&lt;Func&lt;TEntity, TDestination&gt;&gt;</c>
    /// passa por aqui — não chame <c>.Select(projection)</c> diretamente em
    /// <see cref="RepositoryBase{TEntity}"/>.
    /// </remarks>
    protected virtual IQueryable<TDestination> ApplyProjection<TDestination>(IQueryable<TEntity> source, Expression<Func<TEntity, TDestination>> projection)
        => source.Select(projection);

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

    public virtual async ValueTask<bool> DeleteAsync(object[] keys, CancellationToken cancellationToken = default)
    {
        var entity = await DbSet.FindAsync(keys, cancellationToken: cancellationToken);

        if (entity is null)
            return false;

        return await RemoveAsync(entity, cancellationToken);
    }

    public virtual ValueTask<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        => RemoveAsync(entity, cancellationToken);

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

    public async Task ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        var query = await GetQueryable(predicate, true, cancellationToken);

        await query.ExecuteDeleteAsync(cancellationToken);
    }

    #endregion MOD

    #region DISCONNECTED

    public virtual void Update(TEntity entity, object updatingData)
    {
        DbSet.Attach(entity);
        DbSet.Entry(entity).CurrentValues.SetValues(updatingData);
    }

    public virtual void Delete(TEntity entity)
    {
        var entry = DbSet.Attach(entity);
        entry.State = EntityState.Deleted;
    }

    public virtual void Delete(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            Delete(entity);
        }
    }

    #endregion DISCONNECTED

    #region UTIL

    public async ValueTask<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, true, cancellationToken);

        return await query.CountAsync(cancellationToken);
    }

    public virtual async ValueTask<int> CountAsync(ICollection<ColumnFilter> entityFilters, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(null, true, cancellationToken))
            .ApplyFilters(entityFilters);

        return await query.CountAsync(cancellationToken);
    }

    public virtual async ValueTask<int> CountAsync<TDestination>(ICollection<ColumnFilter> entityFilters, ICollection<ColumnFilter> dtoFilters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var entityQuery = (await GetQueryable(null, true, cancellationToken))
            .ApplyFilters(entityFilters);

        var query = ApplyProjection<TDestination>(entityQuery)
            .ApplyFilters(dtoFilters);

        return await query.CountAsync(cancellationToken);
    }

    public async ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, true, cancellationToken);

        return await query.AnyAsync(cancellationToken);
    }

    public virtual async ValueTask<int> CountAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellation = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(null, true, cancellation))
            .ApplyFilters(filters);

        return await query.CountAsync(cancellation);
    }

    public virtual async ValueTask<int> CountAsync<TDestination>(ListCriteria criteria, CancellationToken cancellation = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(null, true, cancellation))
            .ApplyFilters(criteria.Filters)
            .ApplySearch(criteria.Search);

        return await query.CountAsync(cancellation);
    }

    public virtual async ValueTask<int> CountAsync<TDestination>(ICollection<ColumnFilter> entityFilters, ListCriteria criteria, CancellationToken cancellation = default)
        where TDestination : class
    {
        var entityQuery = (await GetQueryable(null, true, cancellation))
            .ApplyFilters(entityFilters);

        var query = ApplyProjection<TDestination>(entityQuery)
            .ApplyFilters(criteria.Filters)
            .ApplySearch(criteria.Search);

        return await query.CountAsync(cancellation);
    }

    public virtual async ValueTask<int> CountAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, ListCriteria criteria, CancellationToken cancellation = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(predicate, true, cancellation))
            .ApplyFilters(criteria.Filters)
            .ApplySearch(criteria.Search);

        return await query.CountAsync(cancellation);
    }

    public virtual async ValueTask<bool> AnyAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellation = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(null, true, cancellation))
            .ApplyFilters(filters);

        return await query.AnyAsync(cancellation);
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

    #region LIST - Expression

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

        return await ApplyProjection(query, projection).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, Expression<Func<TEntity, bool>>? predicate, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = await GetQueryable(predicate, @readonly: true, cancellationToken);

        return await ApplyProjection(query, projection).ApplyCriteria(criteria).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, Expression<Func<TEntity, bool>>? predicate, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var query = ApplyProjection(await GetQueryable(predicate, @readonly: true, cancellationToken), projection);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortSelector)
            : query.OrderByDescending(sortSelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(predicate, true, cancellationToken));

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        ListCriteria criteria,
        CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(predicate, true, cancellationToken))
            .ApplyCriteria(criteria);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        Pagination? pagination,
        Expression<Func<TDestination, dynamic>> sortSelector,
        SortDirection sortDirection = SortDirection.Ascending,
        CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(predicate, false, cancellationToken));

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortSelector)
            : query.OrderByDescending(sortSelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    #endregion LIST - Expression

    #region GET - Keys e Expression

    public virtual async Task<TDestination?> GetByIdAsync<TDestination>(object[] keys, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var predicate = DbSet.EntityType.GetIdExpression<TEntity>(keys);
        var query = await GetQueryable(predicate, true, cancellationToken);

        return await ApplyProjection<TDestination>(query)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(object[] keys, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var predicate = DbSet.EntityType.GetIdExpression<TEntity>(keys);
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
        var query = await GetQueryable(predicate, @readonly, cancellationToken);

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
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(predicate, true, cancellationToken));

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default) where TDestination : class
    {
        var query = await GetQueryable(predicate, true, cancellationToken);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortSelector)
            : query.OrderByDescending(sortSelector);

        return await ApplyProjection<TDestination>(orderedQuery)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetByIdAsync<TDestination>(object[] keys, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var predicate = GetIdExpression(keys);
        var query = await GetQueryable(predicate, true, cancellationToken);

        return await ApplyProjection(query, projection).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, true, cancellationToken);

        return await ApplyProjection(query, projection).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate, true, cancellationToken);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortSelector)
            : query.OrderByDescending(sortSelector);

        return await ApplyProjection(orderedQuery, projection).FirstOrDefaultAsync(cancellationToken);
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

        return await ApplyProjection<TDestination>(query).SingleOrDefaultAsync(cancellationToken);
    }

    #endregion GET - Keys e Expression

    #region LIST - ColumnFilters

    public virtual async Task<List<TEntity>> ToListAsync(ICollection<ColumnFilter> filters, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(null, true, cancellationToken)).ApplyFilters(filters);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> ToListAsync(
        ListCriteria criteria,
        bool @readonly = true,
        CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(null, @readonly, cancellationToken))
            .ApplyCriteria(criteria);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> ToListAsync(
        ICollection<ColumnFilter> filters,
        Pagination? pagination,
        Expression<Func<TEntity, dynamic>> sortKeySelector,
        SortDirection sortDirection = SortDirection.Ascending,
        bool @readonly = true,
        CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(null, true, cancellationToken)).ApplyFilters(filters);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKeySelector)
            : query.OrderByDescending(sortKeySelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    // ===

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        ICollection<ColumnFilter> filters,
        CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(null, true, cancellationToken))
            .ApplyFilters(filters);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        ListCriteria criteria,
        CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(null, true, cancellationToken))
            .ApplyCriteria(criteria);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        ICollection<ColumnFilter> filters,
        Pagination? pagination,
        Expression<Func<TDestination, dynamic>> sortKeySelector,
        SortDirection sortDirection = SortDirection.Ascending,
        CancellationToken cancellationToken = default) where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(null, false, cancellationToken))
            .ApplyFilters(filters);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKeySelector)
            : query.OrderByDescending(sortKeySelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = await GetQueryable(null, true, cancellationToken);

        return await ApplyProjection(query, projection).ApplyFilters(filters).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = await GetQueryable(null, @readonly: true, cancellationToken);

        return await ApplyProjection(query, projection).ApplyCriteria(criteria).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyProjection(await GetQueryable(null, @readonly: true, cancellationToken), projection)
            .ApplyFilters(filters);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKeySelector)
            : query.OrderByDescending(sortKeySelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        ICollection<ColumnFilter> entityFilters,
        ListCriteria criteria,
        CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var entityQuery = (await GetQueryable(null, true, cancellationToken))
            .ApplyFilters(entityFilters);

        var query = ApplyProjection<TDestination>(entityQuery)
            .ApplyCriteria(criteria);

        return await query.ToListAsync(cancellationToken);
    }

    #endregion LIST - ColumnFilters

    #region GET

    public virtual async Task<TDestination?> GetAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(null, true, cancellationToken))
            .ApplyFilters(filters);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = await GetQueryable(null, true, cancellationToken);

        return await ApplyProjection(query, projection).ApplyFilters(filters).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(ICollection<ColumnFilter> filters, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(null, @readonly, cancellationToken);

        return await query.ApplyFilters(filters).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync<TProperty>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TProperty>> includeNavigation, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(null, @readonly, cancellationToken);

        return await query.Include(includeNavigation).ApplyFilters(filters).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TDestination, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(null, true, cancellationToken))
            .ApplyFilters(filters);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKeySelector)
            : query.OrderByDescending(sortKeySelector);

        return await orderedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(ICollection<ColumnFilter> filters, Expression<Func<TEntity, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(null, @readonly, cancellationToken))
            .ApplyFilters(filters);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKeySelector)
            : query.OrderByDescending(sortKeySelector);

        return await orderedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync<TProperty>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TProperty>> includeNavigation, Expression<Func<TEntity, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(null, @readonly, cancellationToken))
            .Include(includeNavigation)
            .ApplyFilters(filters);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKeySelector)
            : query.OrderByDescending(sortKeySelector);

        return await orderedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
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

    public virtual async Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyProjection<TDestination>(await GetQueryable(null, true, cancellationToken))
            .ApplyFilters(filters);

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    #endregion GET
}