using System.Linq.Expressions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;
using Maxsys.Core.Sorting.Selectors;

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

    #region GET

    public virtual async Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        return await _repository.GetAsync<TDestination>(filters, cancellationToken);
    }

    #endregion GET

    #region LIST

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(TFilter filters, ListCriteria criteria, Expression<Func<TDestination, dynamic>> keySelector, CancellationToken cancellationToken = default) where TDestination : class
    {
        return new()
        {
            Count = await _repository.CountAsync(filters, cancellationToken),
            List = await _repository.GetListAsync(filters, criteria, new CustomColumnSelector<TDestination>(keySelector), cancellationToken)
        };
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(TFilter filters, ListCriteria criteria, ISortColumnSelector<TDestination>? sortColumnSelector, CancellationToken cancellationToken = default) where TDestination : class
    {
        return new()
        {
            Count = await _repository.CountAsync(filters, cancellationToken),
            List = await _repository.GetListAsync(filters, criteria, sortColumnSelector ?? new NoneColumnSelector<TDestination>(), cancellationToken)
        };
    }

    public virtual async Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class
    {
        return await _repository.GetListAsync<TDestination>(filters, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, ListCriteria criteria, ISortColumnSelector<TDestination> sortColumnSelector, CancellationToken cancellationToken = default) where TDestination : class
    {
        return await _repository.GetListAsync<TDestination>(filters, criteria, sortColumnSelector, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascendant, CancellationToken cancellationToken = default) where TDestination : class
    {
        return await _repository.GetListAsync<TDestination>(filters, pagination, keySelector, sortDirection, cancellationToken);
    }

    #endregion LIST

    #region QTY

    public virtual async Task<int> CountAsync(TFilter? filters, CancellationToken cancellationToken = default)
        => await _repository.CountAsync(filters ?? new(), cancellationToken);

    public virtual async Task<bool> AnyAsync(TFilter? filters, CancellationToken cancellationToken = default)
        => await _repository.AnyAsync(filters ?? new(), cancellationToken);

    #endregion QTY
}