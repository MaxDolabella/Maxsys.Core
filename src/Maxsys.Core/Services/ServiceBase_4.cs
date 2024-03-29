using System.Linq.Expressions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IService{TKey, TFilter}"/>
public abstract class ServiceBase<TEntity, TRepository, TKey, TFilter>
    : ServiceBase<TEntity, TRepository, TFilter>
    , IService<TKey, TFilter>
    where TKey : notnull
    where TEntity : class
    where TRepository : IRepository<TEntity, TFilter>
    where TFilter : IFilter<TEntity>, new()
{
    protected ServiceBase(TRepository repository)
       : base(repository)
    { }

    protected abstract Expression<Func<TEntity, bool>> IdSelector(TKey id);

    #region GET

    public virtual async Task<TDestination?> GetAsync<TDestination>(TKey id, CancellationToken cancellationToken = default) where TDestination : class
    {
        var item = await _repository.GetAsync<TDestination>(IdSelector(id), cancellationToken);

        OnGetCompleted(item);
        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    #endregion GET

    #region LIST

    public virtual Task<ListDTO<InfoDTO<TKey>>> GetInfoListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default)
        => base.GetListAsync<InfoDTO<TKey>>(filters, criteria, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, CancellationToken cancellationToken = default)
        => base.ToListAsync<InfoDTO<TKey>>(filters, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default)
        => base.ToListAsync<InfoDTO<TKey>>(filters, criteria, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        => base.ToListAsync(filters, pagination, keySelector, sortDirection, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(IEnumerable<TKey> idList, CancellationToken cancellationToken = default)
        => ToInfoListAsync(IdListToFilter(idList), cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(IEnumerable<TKey> idList, ListCriteria criteria, CancellationToken cancellationToken = default)
        => ToInfoListAsync(IdListToFilter(idList), criteria, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(IEnumerable<TKey> idList, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        => ToInfoListAsync(IdListToFilter(idList), pagination, keySelector, sortDirection, cancellationToken);

    public virtual Task<List<TDestination>> ToListAsync<TDestination>(IEnumerable<TKey> idList, CancellationToken cancellationToken = default) where TDestination : class
        => ToListAsync<TDestination>(IdListToFilter(idList), cancellationToken);

    public virtual Task<List<TDestination>> ToListAsync<TDestination>(IEnumerable<TKey> idList, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class
        => ToListAsync<TDestination>(IdListToFilter(idList), criteria, cancellationToken);

    public virtual Task<List<TDestination>> ToListAsync<TDestination>(IEnumerable<TKey> idList, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default) where TDestination : class
        => ToListAsync<TDestination>(IdListToFilter(idList), pagination, keySelector, sortDirection, cancellationToken);

    #endregion LIST

    private static TFilter IdListToFilter(IEnumerable<TKey> idList)
    {
        var filter = new TFilter();

        (filter as IKeyFilter<TKey>)!.IdList = new(idList);

        return filter;
    }
}