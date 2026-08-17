using Maxsys.Core;
using Maxsys.Core.Entities;

namespace Maxsys.EventSourcing;

/// <summary>
/// Entidade de domínio com suporte a Domain Events.
/// Herde desta classe quando a entidade precisar levantar eventos de domínio.
/// </summary>
/// <remarks>
/// Os eventos são enfileirados via <see cref="AddDomainEvent"/> e devem ser publicados
/// pelo service após persistir a entidade, usando <c>IBus.Publish</c>.
/// Após publicar, limpe a fila com <see cref="ClearDomainEvents"/>.
/// <example>
/// <code>
/// public class Order : DomainEntity&lt;int&gt;
/// {
///     public string CustomerId { get; private set; } = null!;
///
///     public static Order Create(string customerId)
///     {
///         var order = new Order { CustomerId = customerId };
///         order.AddDomainEvent(new OrderCreatedEvent(order.Id, customerId));
///         return order;
///     }
///
///     public void Cancel(string reason)
///     {
///         AddDomainEvent(new OrderCancelledEvent(Id, reason));
///     }
/// }
/// </code>
/// </example>
/// </remarks>
public abstract class DomainEntity : Entity
{
    private readonly List<DomainEvent> _domainEvents = [];

    /// <summary>
    /// Eventos de domínio enfileirados. Deve ser ignorado na configuração do EF Core.
    /// </summary>
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>Enfileira um evento para ser publicado após a persistência.</summary>
    public void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    /// <summary>Remove um evento específico da fila.</summary>
    public void RemoveDomainEvent(DomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

    /// <summary>Limpa todos os eventos enfileirados. Chame após publicar via IBus.</summary>
    public void ClearDomainEvents() => _domainEvents.Clear();
}

/// <summary>
/// Entidade de domínio com chave única tipada e suporte a Domain Events.
/// </summary>
/// <typeparam name="TKey">
/// Tipo da chave primária. Deve ser um tipo escalar simples (<see cref="int"/>, <see cref="Guid"/>, <see cref="string"/>, etc.).<br/>
/// <b>Não use tipos compostos</b> — para chave composta, herde de <see cref="DomainEntity"/> (sem TKey)
/// e configure via <c>builder.HasKey(x => new { x.PropA, x.PropB })</c>.
/// </typeparam>
public abstract class DomainEntity<TKey> : DomainEntity, IKey<TKey>
{
#pragma warning disable CS8618
    public virtual TKey Id { get; set; }
#pragma warning restore CS8618
}
