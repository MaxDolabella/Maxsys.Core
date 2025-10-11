using Maxsys.Core.Extensions;
using Maxsys.Core.Sorting;

namespace Maxsys.Core;

/// <summary>
/// Atributo utilizado para indicar a property que será o sort padrão em <see cref="QueryableExtensions.ApplySort"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DefaultSortAttribute : Attribute
{
    public string Property { get; }
    public SortDirection SortDirection { get; }

    public DefaultSortAttribute(string property, SortDirection sortDirection = SortDirection.Ascending)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(property, nameof(property));

        Property = property;
        SortDirection = sortDirection;
    }
}