#if NETSTANDARD2_1
using System.Diagnostics.CodeAnalysis;
#endif

namespace System.Collections.Generic
{
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

#elif NETSTANDARD2_1
        /// <summary>
        /// Determines whether an IEnumerable is structurally equal to the current instance.
        /// </summary>
        /// <typeparam name="T">The type of objects in IEnumerable.</typeparam>
        /// <param name="array">current IEnumerable instance</param>
        /// <param name="otherArray">The IEnumerable to compare with the current instance.</param>
        /// <returns>true if the two IEnumerable are structurally equal; otherwise, false.</returns>
        public static bool ArrayEquals<T>([AllowNull] this IEnumerable<T> array, [AllowNull] IEnumerable<T> otherArray)
        {
            if (array is null && otherArray is null)
                return true;

            if (array is null || otherArray is null)
                return false;

            return (array as IStructuralEquatable).Equals(otherArray
                , StructuralComparisons.StructuralEqualityComparer);
        }
#endif
    }
}