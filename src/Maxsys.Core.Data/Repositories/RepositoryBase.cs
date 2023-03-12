using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Maxsys.DataCore.Interfaces;
using Maxsys.ModelCore.Filtering;
using Maxsys.ModelCore.Listing;
using Maxsys.ModelCore.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.Core.Data;

/// <inheritdoc cref="IRepository"/>
public abstract class RepositoryBase<TContext> : IRepository, IDisposable
    where TContext : DbContext
{
    #region FIELDS

    protected readonly TContext Context;

    #endregion FIELDS

    #region CONSTRUCTOR

    public RepositoryBase(TContext context)
    {
        Context = context;
        ContextId = context.ContextId.InstanceId;
    }

    #endregion CONSTRUCTOR

    #region PROPERTIES

    public Guid Id { get; } = Guid.NewGuid();
    public Guid ContextId { get; }

    #endregion PROPERTIES

    /// <inheritdoc cref="IDisposable"/>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

/// <inheritdoc cref="IRepository{TKey, TEntity}"/>
public abstract class RepositoryBase<TContext, TKey, TEntity> : RepositoryBase<TContext>, IRepository<TKey, TEntity>
    where TContext : DbContext
    where TKey : notnull
    where TEntity : class
{
    protected readonly DbSet<TEntity> DbSet;

    #region CONSTRUCTOR

    public RepositoryBase(TContext context)
        : base(context)
    {
        DbSet = Context.Set<TEntity>();
    }

    #endregion CONSTRUCTOR

    #region IBaseRepository<TEntity>

    public virtual async ValueTask<bool> AddAsync(TEntity entity, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            throw new OperationCanceledException(cancellation);

        var entry = await DbSet.AddAsync(entity, cancellation);

        return entry.State == EntityState.Added;
    }

    public virtual async ValueTask<bool> AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default)
    {
        foreach (var item in entities)
        {
            if (!await AddAsync(item, cancellation))
                return false;
        }

        return true;
    }

    public virtual async ValueTask<bool> DeleteAsync(TKey id, CancellationToken cancellation = default)
    {
        return await DeleteAsync(new object[] { id }, cancellation);
    }

    public virtual async ValueTask<bool> DeleteAsync(object[] id, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            throw new OperationCanceledException(cancellation);

        var entity = await DbSet.FindAsync(id, cancellationToken: cancellation);

        return entity is null || await RemoveAsync(entity, cancellation);
    }

    protected virtual async ValueTask<bool> RemoveAsync(TEntity obj, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            return false;

        var entry = DbSet.Remove(obj);

        return await Task.FromResult(entry.State == EntityState.Deleted);
    }

    public async ValueTask<bool> UpdateAsync(TEntity entity, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            return false;

        var entry = DbSet.Update(entity);

        return await Task.FromResult(entry.State == EntityState.Modified);
    }

    public async ValueTask<bool> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default)
    {
        foreach (var item in entities)
        {
            if (!await UpdateAsync(item, cancellation))
                return false;
        }

        return true;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool @readonly = true, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            throw new OperationCanceledException(cancellation);

        var query = await GetQueryable(null, @readonly);

        return await query.ToListAsync(cancellation);
    }

    public async ValueTask<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            throw new OperationCanceledException(cancellation);

        var query = await GetQueryable(predicate, @readonly);

        //System.Diagnostics.Debug.Write(query.ToQueryString());

        return await query.ToListAsync(cancellation);
    }

    public async ValueTask<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            throw new OperationCanceledException(cancellation);

        var query = await GetQueryable(predicate, @readonly);

        return await query.FirstOrDefaultAsync(cancellation);
    }

    public async ValueTask<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            throw new OperationCanceledException(cancellation);

        var query = await GetQueryable(predicate, true);

        return await query.CountAsync(cancellation);
    }

    public async ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            throw new OperationCanceledException(cancellation);

        var query = await GetQueryable(predicate, true);

        return await query.AnyAsync(cancellation);
    }

    #endregion IBaseRepository<TEntity>

    protected virtual ValueTask<IQueryable<TEntity>> GetQueryable(Expression<Func<TEntity, bool>>? predicate = null, bool @readonly = true, CancellationToken cancellation = default)
    {
        var query = @readonly ? DbSet.AsNoTracking() : DbSet.AsTracking();

        return ValueTask.FromResult(predicate is not null
            ? query.Where(predicate)
            : query);
    }
}

/// <inheritdoc cref="IRepository{TKey, TEntity, TFilter}"/>
public abstract class RepositoryBase<TContext, TKey, TEntity, TFilter> : RepositoryBase<TContext, TKey, TEntity>, IRepository<TKey, TEntity, TFilter>
    where TContext : DbContext
    where TKey : notnull
    where TEntity : class
    where TFilter : IFilter<TEntity>
{
    protected readonly IMapper _mapper;

    #region CONSTRUCTOR

    public RepositoryBase(TContext context, IMapper mapper)
        : base(context)
    {
        _mapper = mapper;
    }

    #endregion CONSTRUCTOR

    #region IBaseRepository<TEntity, TFilter>

    public virtual async ValueTask<int> CountAsync(TFilter filters, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            throw new OperationCanceledException(cancellation);

        return await CountAsync(filters.ToExpression(), cancellation);
    }

    public virtual async ValueTask<bool> AnyAsync(TFilter filters, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            throw new OperationCanceledException(cancellation);

        return await AnyAsync(filters.ToExpression(), cancellation);
    }

    public virtual async Task<IReadOnlyList<TDestination>> GetAsync<TDestination>(
        TFilter filters,
        Criteria criteria,
        ISortColumnSelector<TDestination> sortColumnSelector,
        CancellationToken cancellation = default)
        where TDestination : class
    {
        if (cancellation.IsCancellationRequested)
            throw new OperationCanceledException(cancellation);

        var query = (await GetQueryable(filters.ToExpression(), cancellation: cancellation))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
            .ApplySort(criteria.Sorts, sortColumnSelector)
            .ApplyPagination(criteria.Pagination);

        return await query.ToListAsync(cancellation);
    }

    public virtual async Task<TDestination?> GetFirstAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default) where TDestination : class
    {
        if (cancellation.IsCancellationRequested)
            throw new OperationCanceledException(cancellation);

        var query = (await GetQueryable(predicate, cancellation: cancellation))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.FirstOrDefaultAsync(cancellation);
    }

    #endregion IBaseRepository<TEntity, TFilter>
}

public abstract class RepositoryBase<TContext, TKey, TEntity, TJoin, TFilter> : RepositoryBase<TContext, TKey, TEntity, TFilter>, IRepository<TKey, TEntity, TFilter>
    where TContext : DbContext
    where TKey : notnull
    where TEntity : class
    where TJoin : class
    where TFilter : IFilter<TEntity>
{
    #region CONSTRUCTOR

    public RepositoryBase(TContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    #endregion CONSTRUCTOR

    #region IBaseRepository<TEntity, TFilter>

    public override async Task<IReadOnlyList<TDestination>> GetAsync<TDestination>(
        TFilter filters,
        Criteria criteria,
        ISortColumnSelector<TDestination> sortColumnSelector,
        CancellationToken cancellation = default)
        where TDestination : class
    {
        if (cancellation.IsCancellationRequested)
            throw new OperationCanceledException(cancellation);

        var query = (await GetJoinQueryable(filters.ToExpression(), true, cancellation))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
            .ApplySort(criteria.Sorts, sortColumnSelector)
            .ApplyPagination(criteria.Pagination);

        return await query.ToListAsync(cancellation);
    }

    public override async Task<TDestination?> GetFirstAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default) where TDestination : class
    {
        if (cancellation.IsCancellationRequested)
            throw new OperationCanceledException(cancellation);

        var query = (await GetJoinQueryable(predicate, true, cancellation))
            .ProjectTo<TDestination>(_mapper.ConfigurationProvider);

        return await query.FirstOrDefaultAsync(cancellation);
    }

    #endregion IBaseRepository<TEntity, TFilter>

    protected abstract ValueTask<IQueryable<TJoin>> GetJoinQueryable(Expression<Func<TEntity, bool>>? predicate = null, bool @readonly = true, CancellationToken cancellation = default);
}