using Maxsys.EventSourcing.Abstractions;

namespace Maxsys.EventSourcing;

/// <summary>
/// Record base abstrato para domain events.
/// Preenche <see cref="IDomainEvent.Timestamp"/> e <see cref="IDomainEvent.MessageType"/> automaticamente.
/// </summary>
/// <remarks>
/// <example>
/// <code>
/// // Simples
/// public record OrderCreatedEvent(int OrderId, string CustomerId, decimal Total) : DomainEvent;
///
/// // Com propriedade extra
/// public record UserDeactivatedEvent(int UserId, string Reason) : DomainEvent
/// {
///     public string DeactivatedBy { get; init; } = string.Empty;
/// }
/// </code>
/// </example>
/// </remarks>
public abstract record DomainEvent : IDomainEvent
{
    /// <inheritdoc/>
    public DateTime Timestamp { get; private set; }

    /// <inheritdoc/>
    public string MessageType { get; private set; }

    protected DomainEvent()
    {
        Timestamp = DateTime.UtcNow;
        MessageType = GetType().Name;
    }
}
