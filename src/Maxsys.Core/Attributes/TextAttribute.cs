namespace Maxsys.Core.Attributes;

/// <summary>
/// Atributo utilizado para indicar que uma property é do tipo <c>TEXT</c> no banco,
/// e assim auxiliando na obtenção do selector de ordenação.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class TextAttribute : Attribute
{ }