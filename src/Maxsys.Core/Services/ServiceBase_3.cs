using System.Linq.Expressions;
using Maxsys.Core.Events;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IService{TFilter}"/>
public abstract class ServiceBase<TEntity, TRepository, TFilter>
    : ServiceBase<TEntity, TRepository>, IService<TFilter>
    where TEntity : class
    where TRepository : IRepository<TEntity, TFilter>
    where TFilter : IFilter<TEntity>, new()
{
    protected ServiceBase(TRepository repository) : base(repository)
    { }

    #region GET

    public virtual async Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var item = await _repository.GetAsync<TDestination>(filters, cancellationToken);

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
            Items = await _repository.ToListAsync<TDestination>(filters, criteria, cancellationToken)
        };

        OnGetListCompleted(list);
        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class
    {
        var items = await _repository.ToListAsync<TDestination>(filters, cancellationToken);

        OnToListCompleted(items);
        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class
    {
        var items = await _repository.ToListAsync<TDestination>(filters, criteria, cancellationToken);

        OnToListCompleted(items);
        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default) where TDestination : class
    {
        var items = await _repository.ToListAsync(filters, pagination, keySelector, sortDirection, cancellationToken);

        OnToListCompleted(items);
        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    #endregion LIST

    #region QTY

    public virtual ValueTask<int> CountAsync(TFilter? filters, CancellationToken cancellationToken = default)
        => _repository.CountAsync(filters ?? new(), cancellationToken);

    public virtual ValueTask<bool> AnyAsync(TFilter? filters, CancellationToken cancellationToken = default)
        => _repository.AnyAsync(filters ?? new(), cancellationToken);

    #endregion QTY
}