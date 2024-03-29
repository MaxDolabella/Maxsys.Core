using MediatR;

namespace Maxsys.Core.Messaging.Abstractions;

/// <summary>
/// IRequestHandler{TQuery, TResponse}
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{ }