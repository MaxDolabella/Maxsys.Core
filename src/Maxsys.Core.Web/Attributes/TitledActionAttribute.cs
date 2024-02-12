namespace Maxsys.Core.Web.Attributes;

/// <summary>
/// Utilizado para definir um Título para uma Action.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class TitledActionAttribute : Attribute
{
    public string Title { get; }

    /// <param name="title">é o título, um identificador do endpoint. Ex.: 'CONTACT.LIST'</param>
    /// <exception cref="ArgumentException"/>
    public TitledActionAttribute(string title)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));

        Title = title;
    }
}