namespace Maxsys.Core;

/// <summary>
/// Usado em um literal de enum "SortableColumns" para indicar que esse literal se refere a uma determinada coluna. 
/// Dessa maneira, o literal pode ter um nome diferente da propriedade ordenável, esta sendo informada através de <see cref="Name"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class SortablePropertyAttribute : Attribute
{
    public string Name { get; }

    public SortablePropertyAttribute(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

        Name = name;
    }
}