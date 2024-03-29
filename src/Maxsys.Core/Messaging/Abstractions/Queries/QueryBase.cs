namespace Maxsys.Core.Messaging.Abstractions.Queries;

public abstract record QueryBase<TResponse> : IQuery<TResponse>;

public abstract class QueryHandlerBase<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : QueryBase<TResponse>
{
    public abstract Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
}