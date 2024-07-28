using System.Linq.Expressions;
using Maxsys.Core.Events;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IService{TEntity}"/>
public abstract class ServiceBase<TEntity, TRepository>
    : ServiceBase, IService<TEntity>
    where TEntity : class
    where TRepository : IRepository<TEntity>
{
    protected readonly TRepository _repository;

    protected ServiceBase(TRepository repository) : base()
    {
        _repository = repository;
    }

    #region EVENTS

    public event AsyncEventHandler<ValueEventArgs>? GetCompletedAsync;

    public event AsyncEventHandler<ValueEventArgs>? ToListCompletedAsync;

    public event AsyncEventHandler<ValueEventArgs>? GetListCompletedAsync;

    protected ValueTask OnGetCompletedAsync(object? e, CancellationToken cancellationToken)
    {
        return GetCompletedAsync != null
            ? GetCompletedAsync(this, new ValueEventArgs(e), cancellationToken)
            : ValueTask.CompletedTask;
    }

    protected ValueTask OnToListCompletedAsync(object? e, CancellationToken cancellationToken)
    {
        return ToListCompletedAsync != null
            ? ToListCompletedAsync(this, new ValueEventArgs(e), cancellationToken)
            : ValueTask.CompletedTask;
    }

    protected ValueTask OnGetListCompletedAsync(object? e, CancellationToken cancellationToken)
    {
        return GetListCompletedAsync != null
            ? GetListCompletedAsync(this, new ValueEventArgs(e), cancellationToken)
            : ValueTask.CompletedTask;
    }

    protected virtual void UnsubscribeEvents()
    {
        GetCompletedAsync = null;
        ToListCompletedAsync = null;
        GetListCompletedAsync = null;
    }

    #endregion EVENTS

    #region GET

    public virtual async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetAsync<TDestination>(predicate, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetAsync(predicate, projection, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetByIdAsync<TDestination>(object[] ids, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync<TDestination>(ids, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetSingleOrDefaultAsync<TDestination>(predicate, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetSingleOrThrowsAsync<TDestination>(predicate, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    #endregion GET

    #region LIST

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var items = await _repository.ToListAsync<TDestination>(predicate, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class
    {
        var items = await _repository.ToListAsync<TDestination>(predicate, criteria, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var items = await _repository.ToListAsync(predicate, pagination, sortSelector, sortDirection, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var items = await _repository.ToListAsync(projection, predicate, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var items = await _repository.ToListAsync(projection, predicate, pagination, sortSelector, sortDirection, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var items = await _repository.ToListAsync(projection, predicate, criteria, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync(predicate, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(predicate, criteria, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync(predicate, cancellationToken),
            Items = await _repository.ToListAsync(projection, predicate, criteria, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    #endregion LIST

    #region UTIL

    public virtual ValueTask<bool> IdExistsAsync(object[] ids, CancellationToken cancellationToken = default)
        => _repository.IdExistsAsync(ids, cancellationToken);

    public virtual ValueTask<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _repository.CountAsync(predicate, cancellationToken);
    }

    public virtual ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _repository.AnyAsync(predicate, cancellationToken);
    }

    #endregion UTIL
}