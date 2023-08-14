namespace Maxsys.Core.Entities;

public abstract class Entity<TKey> : IKey<TKey> where TKey : notnull
{
    /// <summary>
    /// The Key of the entity
    /// </summary>
    public virtual TKey Id { get; set; } 
}

/// <summary>
/// Provides a base class for an Entity of key (Key) type <see cref="Guid"/>
/// </summary>
public abstract class Entity : Entity<Guid>
{ }