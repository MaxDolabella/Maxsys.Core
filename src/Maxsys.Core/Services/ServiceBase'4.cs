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
    protected ServiceBase(TRepository repository, ISortColumnSelector<InfoDTO<TKey>> infoSortColumnsSelector)
       : base(repository)
    {
        _infoSortColumnsSelector = infoSortColumnsSelector;
    }

    protected readonly ISortColumnSelector<InfoDTO<TKey>> _infoSortColumnsSelector;

    protected abstract Expression<Func<TEntity, bool>> IdSelector(TKey id);

    #region GET

    public virtual async Task<TDestination?> GetAsync<TDestination>(TKey id, CancellationToken cancellationToken = default) where TDestination : class
    {
        return await _repository.GetAsync<TDestination>(IdSelector(id), cancellationToken);
    }

    #endregion GET

    #region LIST

    public virtual async Task<ListDTO<InfoDTO<TKey>>> GetInfoListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default)
    {
        return new()
        {
            Count = await _repository.CountAsync(filters, cancellationToken),
            List = await _repository.GetListAsync<InfoDTO<TKey>>(filters, criteria, _infoSortColumnsSelector, cancellationToken)
        };
    }

    public virtual async Task<IReadOnlyList<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, CancellationToken cancellationToken = default)
    {
        return await _repository.GetListAsync<InfoDTO<TKey>>(filters, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default)
    {
        return await _repository.GetListAsync<InfoDTO<TKey>>(filters, criteria, _infoSortColumnsSelector, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascendant, CancellationToken cancellationToken = default)
    {
        return await _repository.GetListAsync<InfoDTO<TKey>>(filters, pagination, keySelector, sortDirection, cancellationToken);
    }

    #endregion LIST
}