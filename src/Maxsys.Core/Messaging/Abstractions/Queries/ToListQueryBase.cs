using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;

namespace Maxsys.Core.Messaging.Abstractions.Queries;

public abstract record ToListQueryBase<TEntity, TFilter, TReturn>(TFilter Filter) : QueryBase<List<TReturn>>
    where TEntity : class
    where TFilter : IFilter<TEntity>;

public abstract class ToListQueryHandlerBase<TEntity, TFilter, TQuery, TReturn>
    : QueryHandlerBase<TQuery, List<TReturn>>
    where TEntity : class
    where TFilter : IFilter<TEntity>, new()
    where TQuery : ToListQueryBase<TEntity, TFilter, TReturn>
    where TReturn : class
{
    protected readonly IRepository<TEntity, TFilter> _repository;

    protected ToListQueryHandlerBase(IRepository<TEntity, TFilter> repository)
    {
        _repository = repository;
    }

    public override Task<List<TReturn>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return _repository.ToListAsync<TReturn>(request.Filter, cancellationToken);
    }
}