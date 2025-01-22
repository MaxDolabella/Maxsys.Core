using System.Linq.Expressions;
using AutoMapper;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IService{TEntity, TKey, TFilter}"/>
public abstract class ServiceBase<TEntity, TRepository, TKey, TFilter>
    : ServiceBase<TEntity, TRepository, TKey>
    , IService<TEntity, TKey, TFilter>
    where TEntity : class
    where TRepository : IRepository<TEntity, TFilter>
    where TKey : notnull
    where TFilter : IFilter<TEntity>, new()
{
    protected ServiceBase(TRepository repository, IUnitOfWork uow, IMapper mapper)
       : base(repository, uow, mapper)
    { }

    #region GET

    public virtual async Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetAsync<TDestination>(filters, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetAsync(filters, projection, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetSingleOrDefaultAsync<TDestination>(filters, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetSingleOrThrowsAsync<TDestination>(filters, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    #endregion GET

    #region LIST

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync(filters, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(filters, criteria, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
    {
        var items = await _repository.ToListAsync<TDestination>(filters, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var items = await _repository.ToListAsync<TDestination>(filters, criteria, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var items = await _repository.ToListAsync(filters, pagination, keySelector, sortDirection, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var items = await _repository.ToListAsync(filters, projection, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var items = await _repository.ToListAsync(filters, projection, criteria, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var items = await _repository.ToListAsync(filters, projection, pagination, keySelector, sortDirection, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual Task<ListDTO<InfoDTO<TKey>>> GetInfoListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default)
        => GetListAsync<InfoDTO<TKey>>(filters, criteria, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, CancellationToken cancellationToken = default)
        => ToListAsync<InfoDTO<TKey>>(filters, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default)
        => ToListAsync<InfoDTO<TKey>>(filters, criteria, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        => ToListAsync(filters, pagination, keySelector, sortDirection, cancellationToken);

    #endregion LIST

    #region QTY

    public virtual ValueTask<int> CountAsync(TFilter? filters, CancellationToken cancellationToken = default)
        => _repository.CountAsync(filters ?? new(), cancellationToken);

    public virtual ValueTask<bool> AnyAsync(TFilter? filters, CancellationToken cancellationToken = default)
        => _repository.AnyAsync(filters ?? new(), cancellationToken);

    #endregion QTY
}