using System.Linq.Expressions;
using Maxsys.Core.Events;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IService{TFilter}"/>
public abstract class ServiceBase<TEntity, TRepository, TFilter>
    : ServiceBase, IService<TFilter>
    where TEntity : class
    where TRepository : IRepository<TEntity, TFilter>
    where TFilter : IFilter<TEntity>, new()
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

    public event ValueEventHandler? GetCompleted;

    public event ValueEventHandler? ToListCompleted;

    public event ValueEventHandler? GetListCompleted;

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

    protected void OnGetCompleted(object? e)
    {
        GetCompleted?.Invoke(this, new ValueEventArgs(e));
    }

    protected void OnToListCompleted(object? e)
    {
        ToListCompleted?.Invoke(this, new ValueEventArgs(e));
    }

    protected void OnGetListCompleted(object? e)
    {
        GetListCompleted?.Invoke(this, new ValueEventArgs(e));
    }

    protected virtual void UnsubscribeEvents()
    {
        GetCompletedAsync = null;
        ToListCompletedAsync = null;
        GetListCompletedAsync = null;

        GetCompleted = null;
        ToListCompleted = null;
        GetListCompleted = null;
    }

    #endregion EVENTS

    #region GET

    public virtual async Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var item = await _repository.GetAsync<TDestination>(filters, cancellationToken);

        OnGetCompleted(item);
        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetByIdAsync<TDestination>(object[] ids, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var item = await _repository.GetByIdAsync<TDestination>(ids, cancellationToken);

        OnGetCompleted(item);
        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var item = await _repository.GetSingleOrDefaultAsync<TDestination>(filters, cancellationToken);

        OnGetCompleted(item);
        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var item = await _repository.GetSingleOrThrowsAsync<TDestination>(filters, cancellationToken);

        OnGetCompleted(item);
        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    #endregion GET

    #region LIST

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync(filters, cancellationToken),
            List = await _repository.ToListAsync<TDestination>(filters, criteria, cancellationToken)
        };

        OnGetListCompleted(list);
        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class
    {
        var items = await _repository.ToListAsync<TDestination>(filters, cancellationToken);

        OnToListCompleted(items);
        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class
    {
        var items = await _repository.ToListAsync<TDestination>(filters, criteria, cancellationToken);

        OnToListCompleted(items);
        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascendant, CancellationToken cancellationToken = default) where TDestination : class
    {
        var items = await _repository.ToListAsync(filters, pagination, keySelector, sortDirection, cancellationToken);

        OnToListCompleted(items);
        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    #endregion LIST

    #region QTY

    public virtual async Task<int> CountAsync(TFilter? filters, CancellationToken cancellationToken = default)
        => await _repository.CountAsync(filters ?? new(), cancellationToken);

    public virtual async Task<bool> AnyAsync(TFilter? filters, CancellationToken cancellationToken = default)
        => await _repository.AnyAsync(filters ?? new(), cancellationToken);

    public virtual async ValueTask<bool> IdExistsAsync(object[] ids, CancellationToken cancellationToken = default)
        => await _repository.IdExistsAsync(ids, cancellationToken);

    #endregion QTY
}