using System.Linq.Expressions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IService{TEntity, TKey, TFilter}"/>
public abstract class ServiceBase<TEntity, TRepository, TKey, TFilter>
    : ServiceBase<TEntity, TRepository, TFilter>
    , IService<TEntity, TKey, TFilter>
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

    public virtual async Task<TDestination?> GetAsync<TDestination>(TKey id, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync<TDestination>(id, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(TKey id, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync(id, projection, cancellationToken);

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
        => ToInfoListAsync(FilterFromIdList(idList), cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(IEnumerable<TKey> idList, ListCriteria criteria, CancellationToken cancellationToken = default)
        => ToInfoListAsync(FilterFromIdList(idList), criteria, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(IEnumerable<TKey> idList, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        => ToInfoListAsync(FilterFromIdList(idList), pagination, keySelector, sortDirection, cancellationToken);

    public virtual Task<List<TDestination>> ToListAsync<TDestination>(IEnumerable<TKey> idList, CancellationToken cancellationToken = default)
        => ToListAsync<TDestination>(FilterFromIdList(idList), cancellationToken);

    public virtual Task<List<TDestination>> ToListAsync<TDestination>(IEnumerable<TKey> idList, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class
        => ToListAsync<TDestination>(FilterFromIdList(idList), criteria, cancellationToken);

    public virtual Task<List<TDestination>> ToListAsync<TDestination>(IEnumerable<TKey> idList, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        => ToListAsync(FilterFromIdList(idList), pagination, keySelector, sortDirection, cancellationToken);

    #endregion LIST

    protected static TFilter FilterFromIdList(IEnumerable<TKey> idList)
    {
        TFilter filter = new();

        (filter as IKeyFilter<TKey>)!.IdList = new(idList);

        return filter;
    }

    public Task<List<InfoDTO<TKey>>> ToInfoListAsync(Expression<Func<TEntity, bool>> predicate, ListCriteria criteria, CancellationToken cancellationToken = default)
    {
        return base.ToListAsync<InfoDTO<TKey>>(predicate, criteria, cancellationToken);
    }

    public Task<List<InfoDTO<TKey>>> ToInfoListAsync(Expression<Func<TEntity, bool>> predicate, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        return base.ToListAsync(predicate, pagination, sortSelector, sortDirection, cancellationToken);
    }

    public Task<List<InfoDTO<TKey>>> ToInfoListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return base.ToListAsync<InfoDTO<TKey>>(predicate, ListCriteria.Empty, cancellationToken);
    }
}