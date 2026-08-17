using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Maxsys.Core.Extensions;

/// <summary>
/// Provides extension methods to <see cref="Type"/>
/// </summary>
public static class TypeExtensions
{
    extension(Type? givenType)
    {
        /// <summary>
        /// Determines whether the <paramref name="genericType"/> is assignable from
        /// <paramref name="givenType"/> taking into account generic definitions
        /// </summary>
        /// <remarks>Adapted from this <see href="https://glacius.tmont.com/articles/determining-if-an-open-generic-type-isassignablefrom-a-type">article</see>.</remarks>
        public bool IsAssignableToGenericType(Type? genericType)
        {
            if (givenType is null || genericType is null)
            {
                return false;
            }

            return givenType == genericType
              || MapsToGenericTypeDefinition(givenType, genericType)
              || HasInterfaceThatMapsToGenericTypeDefinition(givenType, genericType)
              || givenType.BaseType.IsAssignableToGenericType(genericType);
        }

        public bool TryGetAttribute<T>([NotNullWhen(true)] out T? value) where T : Attribute
        {
            value = givenType?.GetCustomAttribute<T>();

            return value is not null;
        }
    }

    private static bool HasInterfaceThatMapsToGenericTypeDefinition(Type givenType, Type genericType)
    {
        return givenType
          .GetInterfaces()
          .Where(it => it.IsGenericType)
          .Any(it => it.GetGenericTypeDefinition() == genericType);
    }

    private static bool MapsToGenericTypeDefinition(Type givenType, Type genericType)
    {
        return genericType.IsGenericTypeDefinition
          && givenType.IsGenericType
          && givenType.GetGenericTypeDefinition() == genericType;
    }
}