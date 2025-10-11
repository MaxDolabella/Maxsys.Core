namespace Maxsys.Core.Web.Attributes;

/// <summary>
/// Utilizado para definir um Título para uma Action.
/// </summary>
[Obsolete("Utilizar ActionIdentifierAttribute", true)]
[AttributeUsage(AttributeTargets.Method)]
public class TitledActionAttribute : ActionIdentifierAttribute
{
    /// <param name="title">é o título, um identificador do endpoint. Ex.: 'CONTACT.LIST'</param>
    /// <exception cref="ArgumentException"/>
    public TitledActionAttribute(string title) : base(title) { }
}