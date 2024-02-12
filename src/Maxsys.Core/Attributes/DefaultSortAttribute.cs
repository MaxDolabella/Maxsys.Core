using Maxsys.Core.Extensions;

namespace Maxsys.Core;

/// <summary>
/// Atributo utilizado para indicar a property que será o sort padrão em <see cref="QueryableExtensions.ApplySort"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DefaultSortAttribute : Attribute
{
    public string Property { get; }

    public DefaultSortAttribute(string property)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(property, nameof(property));

        Property = property;
    }
}