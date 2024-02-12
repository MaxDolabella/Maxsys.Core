namespace Maxsys.Core;

/// <summary>
/// Especifica que um objeto é ordenável através de um enum contendo as colunas ordenáveis.
/// </summary>

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SortableAttribute : Attribute
{
    public Type SortColumnsType { get; }

    public SortableAttribute(Type sortColumnsType)
    {
        ArgumentNullException.ThrowIfNull(sortColumnsType, nameof(sortColumnsType));

        if (!sortColumnsType.IsEnum)
            throw new ArgumentException("Argument must be an Enum type.", nameof(sortColumnsType));

        SortColumnsType = sortColumnsType;
    }
}

/// <inheritdoc cref="SortableAttribute"/>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class SortableAttribute<T> : SortableAttribute
    where T : Enum
{
    public SortableAttribute() : base(typeof(T))
    { }
}