using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Maxsys.Core.Entities;

public abstract class Entity<TKey> : IKey<TKey>, IEquatable<Entity<TKey>>, IEqualityComparer<Entity<TKey>>
    where TKey : notnull
{
    /// <summary>
    /// The Key of the entity
    /// </summary>
    public virtual TKey Id { get; set; }

    public virtual bool Equals(Entity<TKey>? other)
    {
        return other is not null && Id.Equals(other.Id);
    }

    public virtual bool Equals(Entity<TKey>? x, Entity<TKey>? y)
    {
        return (x, y) == (null, null) || (x is not null && x.Equals(y)) || (y is not null && y.Equals(x));
    }

    public virtual int GetHashCode([DisallowNull] Entity<TKey> obj)
    {
        return obj.Id.GetHashCode();
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity<TKey> other && Equals(other);
    }
}

/// <summary>
/// Provides a base class for an Entity of key (Key) type <see cref="Guid"/>
/// </summary>
public abstract class Entity : Entity<Guid>
{ }