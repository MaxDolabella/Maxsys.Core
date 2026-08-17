namespace Maxsys.Messaging.Abstractions;

/// <summary>Handler para commands com retorno.</summary>
public interface ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<TResponse> HandleAsync(TCommand command, CancellationToken ct);
}

/// <summary>Handler para commands sem retorno.</summary>
public interface ICommandHandler<TCommand>
    where TCommand : ICommand
{
    Task HandleAsync(TCommand command, CancellationToken ct);
}