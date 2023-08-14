using System.Linq.Expressions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;
using Maxsys.Core.Sorting.Selectors;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IReadService{TFilter}"/>
public abstract class ReadServiceBase<TEntity, TRepository, TFilter> : ServiceBase, IReadService<TFilter>
    where TEntity : class
    where TRepository : IRepository<TEntity, TFilter>
    where TFilter : IFilter<TEntity>, new()
{
    protected readonly TRepository _repository;

    protected abstract Expression<Func<TEntity, bool>> IdSelector(object id);

    public ReadServiceBase(TRepository repository) : base()
    {
        _repository = repository;
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(object id, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        return await _repository.GetAsync<TDestination>(IdSelector(id), cancellationToken);
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(TFilter filters, ListCriteria criteria, ISortColumnSelector<TDestination>? sortColumnSelector = null, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        return new()
        {
            Count = await _repository.CountAsync(filters, cancellationToken),
            List = await _repository.GetListAsync(filters, criteria, sortColumnSelector ?? new NoneColumnSelector<TDestination>(), cancellationToken)
        };
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(TFilter filters, ListCriteria criteria, Expression<Func<TDestination, dynamic>>? columnSelector = null, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        ISortColumnSelector<TDestination> sortColumnSelector = columnSelector is null
            ? new NoneColumnSelector<TDestination>()
            : new CustomColumnSelector<TDestination>(columnSelector);

        return await GetListAsync(filters, criteria, sortColumnSelector, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TDestination>> GetListAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        return await _repository.GetListAsync<TDestination>(filters, cancellationToken);
    }

    public virtual async Task<int> CountAsync(TFilter? filters, CancellationToken cancellationToken = default)
        => await _repository.CountAsync(filters ?? new(), cancellationToken);

    public virtual async Task<bool> AnyAsync(TFilter? filters, CancellationToken cancellationToken = default)
        => await _repository.AnyAsync(filters ?? new(), cancellationToken);
}