using Maxsys.Messaging.Abstractions;

namespace Maxsys.EventSourcing.Abstractions;

/// <summary>
/// Contrato para eventos de domínio DDD.
/// Representa algo que <b>aconteceu</b> no domínio — imutável, nomeado no passado.
/// Estende <see cref="IEvent"/>, permitindo publicação via <c>IBus.Publish</c>.
/// </summary>
/// <remarks>
/// Prefira herdar de <see cref="DomainEvent"/> (record base) em vez de implementar
/// esta interface diretamente — <c>DomainEvent</c> preenche <see cref="Timestamp"/>
/// e <see cref="MessageType"/> automaticamente.
/// <example>
/// <code>
/// public record OrderCreatedEvent(int OrderId, string CustomerId) : DomainEvent;
/// </code>
/// </example>
/// </remarks>
public interface IDomainEvent : IEvent
{
    /// <summary>Momento UTC em que o evento ocorreu.</summary>
    DateTime Timestamp { get; }

    /// <summary>Nome do tipo do evento. Usado para persistência e roteamento.</summary>
    string MessageType { get; }
}
