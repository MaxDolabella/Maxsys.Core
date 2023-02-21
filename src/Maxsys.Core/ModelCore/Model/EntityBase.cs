using System;
using System.Collections.Generic;

namespace Maxsys.ModelCore;

/// <summary>
/// Provides a base class for an Entity of key type <typeparamref name="TKey"/>
/// </summary>
/// <typeparam name="TKey">Type of key (Id). <para/>Example: TKey=Guid => public Guid Id { get; protected set; }</typeparam>
public abstract class EntityBase<TKey> : IEquatable<EntityBase<TKey>?>
    where TKey : notnull
{
    /// <summary>
    /// Key of this entity.
    /// </summary>
    public virtual TKey Id { get; protected set; }

    #region Overrides

    /// <inheritdoc/>
    public override string ToString()
    {
        return GetType().Name + " [Id=" + Id + "]";
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return Equals(obj as EntityBase<TKey>);
    }

    /// <inheritdoc/>
    public bool Equals(EntityBase<TKey>? other)
    {
        return other is not null &&
               EqualityComparer<TKey>.Default.Equals(Id, other.Id);
    }

    /// <inheritdoc/>
    public static bool operator ==(EntityBase<TKey>? left, EntityBase<TKey>? right)
    {
        return EqualityComparer<EntityBase<TKey>>.Default.Equals(left, right);
    }

    /// <inheritdoc/>
    public static bool operator !=(EntityBase<TKey>? left, EntityBase<TKey>? right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    #endregion Overrides
}