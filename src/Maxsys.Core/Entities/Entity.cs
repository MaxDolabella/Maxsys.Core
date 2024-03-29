using Maxsys.Core.EventSourcing;

namespace Maxsys.Core.Entities;

public abstract class Entity
{
    private readonly List<DomainEvent> _domainEvents = [];

    /// <summary>
    /// Must be ignored in entity configuration
    /// </summary>
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(DomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

    public void ClearDomainEvent() => _domainEvents.Clear();
}

public abstract class Entity<TKey> : Entity, IKey<TKey>
{
    // TODO estudar comportamento quando required for aplicado.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public virtual TKey Id { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}