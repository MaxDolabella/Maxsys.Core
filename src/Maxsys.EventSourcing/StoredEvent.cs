using Maxsys.Core.Extensions;
using Maxsys.EventSourcing.Abstractions;

namespace Maxsys.EventSourcing;

/// <summary>
/// Representação serializada de um domain event para persistência.
/// Criado via <see cref="Create{T}"/>.
/// </summary>
/// <remarks>
/// <example>
/// <code>
/// var evt = new OrderCreatedEvent(42, "customer-1", 199.90m);
/// var stored = StoredEvent.Create(evt);
///
/// Console.WriteLine(stored.EventType);  // "OrderCreatedEvent"
/// Console.WriteLine(stored.Data);       // JSON do evento
/// </code>
/// </example>
/// </remarks>
public sealed class StoredEvent
{
    /// <summary>Identificador único do evento armazenado.</summary>
    public Guid Id { get; private set; }

    /// <summary>Momento UTC em que o evento foi armazenado.</summary>
    public DateTime Timestamp { get; private set; }

    /// <summary>Nome do tipo do evento. Equivale a <see cref="IDomainEvent.MessageType"/>.</summary>
    public string EventType { get; private set; } = string.Empty;

    /// <summary>Payload do evento serializado como JSON.</summary>
    public string Data { get; private set; } = string.Empty;

    public StoredEvent() { }

    /// <summary>
    /// Cria um <see cref="StoredEvent"/> a partir de um domain event, serializando seu payload em JSON.
    /// </summary>
    public static StoredEvent Create<T>(T @event) where T : class, IDomainEvent
    {
        ArgumentNullException.ThrowIfNull(@event, nameof(@event));

        return new StoredEvent
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            EventType = @event.MessageType,
            Data = @event.ToJson()!
        };
    }
}
