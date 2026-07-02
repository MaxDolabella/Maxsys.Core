using Maxsys.Messaging.Abstractions;

namespace Maxsys.Messaging.Internal;

/// <summary>
/// Implementação pública de IBus que delega para IMessageDispatcher.
/// Injete IBus nos controllers e services da aplicação.
/// </summary>
internal sealed class MaxsysBus : IBus
{
    private readonly IMessageDispatcher _dispatcher;

    public MaxsysBus(IMessageDispatcher dispatcher) => _dispatcher = dispatcher;

    public Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken)
        => _dispatcher.SendAsync(command, cancellationToken);

    public Task SendAsync(ICommand command, CancellationToken cancellationToken)
        => _dispatcher.SendAsync(command, cancellationToken);

    public Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
        => _dispatcher.SendAsync(query, cancellationToken);

    public Task Publish<TEvent>(TEvent @event, CancellationToken ct = default)
        where TEvent : class, IEvent
        => _dispatcher.PublishAsync(@event, ct);
}