namespace Maxsys.Messaging.Abstractions.Queries;

/// <summary>Record base para queries. Use record para comparação por valor e imutabilidade natural.</summary>
public abstract record QueryBase<TResponse> : IQuery<TResponse>;

/// <summary>Classe base para handlers de queries. Implemente HandleAsync.</summary>
public abstract class QueryHandlerBase<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : QueryBase<TResponse>
{
    public abstract Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken);
}