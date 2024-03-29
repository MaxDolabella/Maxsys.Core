using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;

namespace Maxsys.Core.Messaging.Abstractions.Queries;

public abstract record GetListQueryBase<TEntity, TFilter, TReturn>(TFilter Filter, ListCriteria ListCriteria) : QueryBase<ListDTO<TReturn>>
    where TEntity : class
    where TFilter : IFilter<TEntity>;

public abstract class GetListQueryHandlerBase<TEntity, TFilter, TQuery, TReturn>
    : QueryHandlerBase<TQuery, ListDTO<TReturn>>
    where TEntity : class
    where TFilter : IFilter<TEntity>, new()
    where TQuery : GetListQueryBase<TEntity, TFilter, TReturn>
    where TReturn : class
{
    protected readonly IRepository<TEntity, TFilter> _repository;

    protected GetListQueryHandlerBase(IRepository<TEntity, TFilter> repository)
    {
        _repository = repository;
    }

    public override async Task<ListDTO<TReturn>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return new ListDTO<TReturn>
        {
            Count = await _repository.CountAsync(request.Filter, cancellationToken),
            Items = await _repository.ToListAsync<TReturn>(request.Filter, request.ListCriteria, cancellationToken)
        };
    }
}