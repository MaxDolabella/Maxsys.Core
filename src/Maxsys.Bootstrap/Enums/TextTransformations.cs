using System.ComponentModel;

namespace Maxsys.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/utilities/text/#text-transform">Documentation</see>
/// <list type="bullet">
/// <item>
///     <term>0.<see cref="LowerCase"/></term>
///     <description>lowercased text</description>
/// </item>
/// <item>
///     <term>1.<see cref="UpperCase"/></term>
///     <description>UPPERCASED TEXT.</description>
/// </item>
/// <item>
///     <term>2.<see cref="Capitalize"/></term>
///     <description>CapiTaliZed Text.</description>
/// </item>
/// </list>
/// </summary>
public enum TextTransformations : byte
{
    [Description("")]
    None = 0,

    /// <summary>
    /// <c>text-lowercase</c>: lowercased text
    /// </summary>
    [Description("text-lowercase")]
    LowerCase = 1,

    /// <summary>
    /// <c>text-uppercase</c>: UPPERCASED TEXT
    /// </summary>
    [Description("text-uppercase")]
    UpperCase = 2,

    /// <summary>
    /// <c>text-capitalize</c>: CapiTaliZed Text
    /// </summary>
    [Description("text-capitalize")]
    Capitalize = 3
}