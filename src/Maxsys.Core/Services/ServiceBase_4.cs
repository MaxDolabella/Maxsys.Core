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

    public virtual async Task<ListDTO<InfoDTO<TKey>>> GetInfoListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default)
    {
        return await base.GetListAsync<InfoDTO<TKey>>(filters, criteria, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, CancellationToken cancellationToken = default)
    {
        return await base.ToListAsync<InfoDTO<TKey>>(filters, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default)
    {
        return await base.ToListAsync<InfoDTO<TKey>>(filters, criteria, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascendant, CancellationToken cancellationToken = default)
    {
        return await base.ToListAsync(filters, pagination, keySelector, sortDirection, cancellationToken);
    }

    #endregion LIST
}