using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Maxsys.Core.Extensions;

/// <summary>
/// Provides extension methods to <see cref="Type"/>
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Determines whether the <paramref name="genericType"/> is assignable from
    /// <paramref name="givenType"/> taking into account generic definitions
    /// </summary>
    /// <remarks>Adapted from this <see href="https://glacius.tmont.com/articles/determining-if-an-open-generic-type-isassignablefrom-a-type">article</see>.</remarks>
    public static bool IsAssignableToGenericType(this Type? givenType, Type? genericType)
    {
        if (givenType is null || genericType is null)
        {
            return false;
        }

        return givenType == genericType
          || givenType.MapsToGenericTypeDefinition(genericType)
          || givenType.HasInterfaceThatMapsToGenericTypeDefinition(genericType)
          || givenType.BaseType.IsAssignableToGenericType(genericType);
    }

    private static bool HasInterfaceThatMapsToGenericTypeDefinition(this Type givenType, Type genericType)
    {
        return givenType
          .GetInterfaces()
          .Where(it => it.IsGenericType)
          .Any(it => it.GetGenericTypeDefinition() == genericType);
    }

    private static bool MapsToGenericTypeDefinition(this Type givenType, Type genericType)
    {
        return genericType.IsGenericTypeDefinition
          && givenType.IsGenericType
          && givenType.GetGenericTypeDefinition() == genericType;
    }

    public static bool TryGetAttribute<T>(this Type? type, [NotNullWhen(true)] out T? value) where T : Attribute
    {
        value = type?.GetCustomAttribute<T>();

        return value is not null;
    }
}