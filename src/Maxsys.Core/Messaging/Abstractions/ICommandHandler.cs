using MediatR;

namespace Maxsys.Core.Messaging.Abstractions;

/// <summary>
/// Apagar summary: IRequestHandler{TCommand, TResponse}
/// </summary>
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{ }

/// <summary>
/// Apagar summary: IRequestHandler{TCommand}
/// </summary>
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand
{ }