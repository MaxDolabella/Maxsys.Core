using Maxsys.Core.Audit;

namespace Maxsys.Core.EventSourcing;

public abstract record AuditableDomainEvent<TKey>(TKey Id) : DomainEvent
{
    public void SetAudit(AuditLog audit) => Audit = audit;
    public AuditLog Audit { get; protected set; } = new();
}