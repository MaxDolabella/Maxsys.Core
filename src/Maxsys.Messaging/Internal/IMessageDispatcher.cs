using Maxsys.Messaging.Abstractions;

namespace Maxsys.Messaging.Internal;

/// <summary>
/// Engine interna de dispatch. Implementação padrão: MaxsysMediator.
/// Pode ser substituída via IBus customizado — não é necessário trocar este contrato diretamente.
/// </summary>
internal interface IMessageDispatcher
{
    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct);

    Task SendAsync(ICommand command, CancellationToken ct);

    Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken ct);

    Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct) where TEvent : class, IEvent;
}