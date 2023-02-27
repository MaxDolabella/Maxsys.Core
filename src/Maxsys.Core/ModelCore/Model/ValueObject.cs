using System;
using System.Collections.Generic;

namespace Maxsys.ModelCore;

public abstract class ValueObject<T> : IEquatable<ValueObject<T>?>
{
    #region Overrides

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return Equals(obj as ValueObject<T>);
    }

    /// <inheritdoc/>
    public abstract bool Equals(ValueObject<T>? other);

    /// <inheritdoc/>
    public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right)
    {
        return EqualityComparer<ValueObject<T>>.Default.Equals(left, right);
    }

    /// <inheritdoc/>
    public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public abstract int GetHashCode();

    #endregion Overrides
}