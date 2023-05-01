#if NET5_0_OR_GREATER

using System.Diagnostics.CodeAnalysis;

#endif

using System.Collections.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Maxsys.Core.Extensions;

/// <summary>
/// Contains extension methods for <see cref="IEnumerable{T}"/> classes.
/// </summary>
public static class IEnumerableExtensions
{
#if NETSTANDARD2_0

    /// <summary>
    /// Determines whether an IEnumerable is structurally equal to the current instance.
    /// </summary>
    /// <typeparam name="T">The type of objects in IEnumerable.</typeparam>
    /// <param name="array">current IEnumerable instance</param>
    /// <param name="otherArray">The IEnumerable to compare with the current instance.</param>
    /// <returns>true if the two IEnumerable are structurally equal; otherwise, false.</returns>
    public static bool ArrayEquals<T>(this IEnumerable<T> array, IEnumerable<T> otherArray)
    {
        return !(otherArray is null)
            && (array as IStructuralEquatable).Equals(otherArray, StructuralComparisons.StructuralEqualityComparer);
    }

#elif NET5_0_OR_GREATER

    /// <summary>
    /// Determines whether an IEnumerable is structurally equal to the current instance.
    /// </summary>
    /// <typeparam name="T">The type of objects in IEnumerable.</typeparam>
    /// <param name="array">current IEnumerable instance</param>
    /// <param name="otherArray">The IEnumerable to compare with the current instance.</param>
    /// <returns>true if the two IEnumerable are structurally equal; otherwise, false.</returns>
    public static bool ArrayEquals<T>([AllowNull] this IEnumerable<T>? array, [AllowNull] IEnumerable<T>? otherArray)
    {
        if (array is null && otherArray is null)
            return true;

        if (array is null || otherArray is null)
            return false;

        return (array as IStructuralEquatable)?.Equals(otherArray
            , StructuralComparisons.StructuralEqualityComparer) == true;
    }

#endif

    /// <summary>
    ///  Creates an <see cref="ObservableCollection{TSource}"/> from an <see cref="IEnumerable{TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{TSource}"/> to create a <see cref="ObservableCollection{TSource}"/> from.</param>
    /// <returns>An <see cref="ObservableCollection{TSource}"/> that contains elements from the input sequence.</returns>
    /// <exception cref="ArgumentNullException">source is null.</exception>
    public static ObservableCollection<TSource> ToObservableCollection<TSource>(this IEnumerable<TSource> source)
    {
        return source is null
            ? throw new ArgumentNullException(nameof(source))
            : new ObservableCollection<TSource>(source);
    }

    /// <summary>
    ///  Creates an <see cref="ReadOnlyObservableCollection{TSource}"/> from an <see cref="IEnumerable{TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{TSource}"/> to create a <see cref="ReadOnlyObservableCollection{TSource}"/> from.</param>
    /// <returns>An <see cref="ReadOnlyObservableCollection{TSource}"/> that contains elements from the input sequence.</returns>
    /// <exception cref="ArgumentNullException">source is null.</exception>
    public static ReadOnlyObservableCollection<TSource> ToReadOnlyObservableCollection<TSource>(this IEnumerable<TSource> source)
    {
        return source is null
           ? throw new ArgumentNullException(nameof(source))
           : new ReadOnlyObservableCollection<TSource>(source is ObservableCollection<TSource> obs ? obs : source.ToObservableCollection());
    }
}