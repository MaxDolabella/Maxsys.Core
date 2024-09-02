using System.ComponentModel;

namespace Maxsys.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/utilities/text/#font-size">Documentation</see>
/// <list type="bullet">
/// <item>
///     <term>1.<see cref="Size1"/></term>
///     <description>fs-1</description>
/// </item>
/// <item>
///     <term>2.<see cref="Size2"/></term>
///     <description>fs-2</description>
/// </item>
/// <item>
///     <term>3.<see cref="Size3"/></term>
///     <description>fs-3</description>
/// </item>
/// <item>
///     <term>4.<see cref="Size4"/></term>
///     <description>fs-4</description>
/// </item>
/// <item>
///     <term>5.<see cref="Size5"/></term>
///     <description>fs-5</description>
/// </item>
/// <item>
///     <term>6.<see cref="Size6"/></term>
///     <description>fs-6</description>
/// </item>
/// </list>
/// </summary>
public enum FontSizes : byte
{
    [Description("")]
    None = 0,

    /// <summary>
    /// fs-1
    /// </summary>
    [Description("fs-1")]
    Size1 = 1,

    /// <summary>
    /// fs-2
    /// </summary>
    [Description("fs-2")]
    Size2 = 2,

    /// <summary>
    /// fs-3
    /// </summary>
    [Description("fs-3")]
    Size3 = 3,

    /// <summary>
    /// fs-4
    /// </summary>
    [Description("fs-4")]
    Size4 = 4,

    /// <summary>
    /// fs-5
    /// </summary>
    [Description("fs-5")]
    Size5 = 5,

    /// <summary>
    /// fs-6
    /// </summary>
    [Description("fs-6")]
    Size6 = 6
}