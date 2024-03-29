using MediatR;

namespace Maxsys.Core.Messaging.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{ }