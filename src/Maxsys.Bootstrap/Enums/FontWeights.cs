using System.ComponentModel;

namespace Maxsys.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/utilities/text/#font-weight-and-italics">Documentation</see>
/// <list type="bullet">
/// <item>
///     <term>1.<see cref="Bold"/></term>
///     <description>fw-bold</description>
/// </item>
/// <item>
///     <term>2.<see cref="Bolder"/></term>
///     <description>fw-bolder - relative to the parent element</description>
/// </item>
/// <item>
///     <term>3.<see cref="Semibold"/></term>
///     <description>fw-semibold</description>
/// </item>
/// <item>
///     <term>4.<see cref="Medium"/></term>
///     <description>fw-medium</description>
/// </item>
/// <item>
///     <term>5.<see cref="Normal"/></term>
///     <description>fw-normal</description>
/// </item>
/// <item>
///     <term>6.<see cref="Light"/></term>
///     <description>fw-light</description>
/// </item>
/// <item>
///     <term>7.<see cref="Lighter"/></term>
///     <description>fw-lighter - relative to the parent element</description>
/// </item>
/// </list>
/// </summary>
public enum FontWeights : byte
{
    [Description("")]
    None = 0,

    /// <summary>
    /// fw-bold
    /// </summary>
    [Description("fw-bold")]
    Bold = 1,

    /// <summary>
    /// fw-bolder
    /// </summary>
    [Description("fw-bolder")]
    Bolder = 2,

    /// <summary>
    /// fw-semibold
    /// </summary>
    [Description("fw-semibold")]
    Semibold = 3,

    /// <summary>
    /// fw-medium
    /// </summary>
    [Description("fw-medium")]
    Medium = 4,

    /// <summary>
    /// fw-normal
    /// </summary>
    [Description("fw-normal")]
    Normal = 5,

    /// <summary>
    /// fw-light
    /// </summary>
    [Description("fw-light")]
    Light = 6,

    /// <summary>
    /// fw-lighter
    /// </summary>
    [Description("fw-lighter")]
    Lighter = 7
}