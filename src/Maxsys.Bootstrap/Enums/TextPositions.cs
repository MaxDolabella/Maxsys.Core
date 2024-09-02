using System.ComponentModel;

namespace Maxsys.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/utilities/text/#text-alignment">Documentation</see>
/// <list type="bullet">
/// <item>
///     <term>0.<see cref="Start"/></term>
///     <description>Start aligned text on all viewport sizes.</description>
/// </item>
/// <item>
///     <term>1.<see cref="Center"/></term>
///     <description>Center aligned text on all viewport sizes.</description>
/// </item>
/// <item>
///     <term>2.<see cref="End"/></term>
///     <description>End aligned text on all viewport sizes.</description>
/// </item>
/// </list>
/// </summary>
public enum TextPositions : byte
{
    [Description("")]
    None = 0,

    /// <summary>
    /// <c>text-start</c>: Start aligned text on all viewport sizes
    /// </summary>
    [Description("text-start")]
    Start = 1,

    /// <summary>
    /// <c>text-center</c>: Center aligned text on all viewport sizes
    /// </summary>
    [Description("text-center")]
    Center = 2,

    /// <summary>
    /// <c>text-end</c>: End aligned text on all viewport sizes
    /// </summary>
    [Description("text-end")]
    End = 3
}