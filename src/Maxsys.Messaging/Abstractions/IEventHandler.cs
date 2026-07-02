namespace Maxsys.Messaging.Abstractions;

/// <summary>
/// Handler para eventos publicados via IBus.Publish.
/// Múltiplos handlers podem ser registrados para o mesmo evento — todos executados em paralelo.
/// Use diretamente para eventos de aplicação. Para eventos de domínio DDD, use <see cref="IDomainEventHandler{TEvent}"/>.
/// </summary>
public interface IEventHandler<in TEvent>
    where TEvent : class, IEvent
{
    Task HandleAsync(TEvent @event, CancellationToken ct);
}
