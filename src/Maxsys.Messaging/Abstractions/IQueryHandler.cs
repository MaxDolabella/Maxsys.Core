namespace Maxsys.Messaging.Abstractions;

/// <summary>Handler para queries com retorno.</summary>
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<TResponse> HandleAsync(TQuery query, CancellationToken ct);
}