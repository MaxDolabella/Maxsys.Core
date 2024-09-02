using AutoMapper;
using Maxsys.Core.Interfaces.Repositories;

namespace Maxsys.Core.Messaging.Abstractions.Queries;

public abstract record GetByIdQueryBase<TKey, TResponse>(TKey Id) : QueryBase<TResponse>;

public abstract class GetByIdQueryHandlerBase<TKey, TEntity, TQuery, TResponse>
    : QueryHandlerBase<TQuery, TResponse>
    where TEntity : class
    where TQuery : GetByIdQueryBase<TKey, TResponse>
    where TResponse : class
{
    protected readonly IRepository<TEntity> _repository;
    protected readonly IMapper _mapper;

    protected GetByIdQueryHandlerBase(IRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken)
    {
        return _repository.GetByIdAsync<TResponse>([query.Id!], cancellationToken);
    }
}