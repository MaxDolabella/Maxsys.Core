using Maxsys.Core.Audit;

namespace Maxsys.EventSourcing;

/// <summary>
/// Domain event com suporte a <see cref="AuditLog"/>.
/// Use quando o evento precisa rastrear quem o causou.
/// </summary>
/// <typeparam name="TKey">Tipo do Id da entidade que gerou o evento.</typeparam>
/// <remarks>
/// <example>
/// <code>
/// public record ProductPriceChangedEvent(int ProductId, decimal OldPrice, decimal NewPrice)
///     : AuditableDomainEvent&lt;int&gt;(ProductId);
///
/// // No service, antes de publicar:
/// var evt = new ProductPriceChangedEvent(product.Id, oldPrice, newPrice);
/// evt.SetAudit(currentUserAudit);
/// await _bus.Publish(evt, ct);
/// </code>
/// </example>
/// </remarks>
public abstract record AuditableDomainEvent<TKey>(TKey Id) : DomainEvent
{
    /// <summary>Informações de auditoria. Preencha via <see cref="SetAudit"/> antes de publicar.</summary>
    public AuditLog Audit { get; protected set; } = new();

    /// <summary>Define o <see cref="AuditLog"/> do evento.</summary>
    public void SetAudit(AuditLog audit) => Audit = audit;
}
