using System.ComponentModel;

namespace Maxsys.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/utilities/flex/#justify-content">Documentation</see>
///
/// <para>Use justify-content utilities on flexbox containers to change the alignment of flex items on the
/// main axis (the x-axis to start, y-axis if flex-direction: column). Choose from start (browser default),
/// end, center, between, around, or evenly.</para>
///
/// <list type="bullet">
/// <item>
///     <term>1.<see cref="Start"/></term>
///     <description>Pack items from the start.</description>
/// </item>
/// <item>
///     <term>2.<see cref="End"/></term>
///     <description>Pack items from the end.</description>
/// </item>
/// <item>
///     <term>3.<see cref="Center"/></term>
///     <description>Pack items around the center.</description>
/// </item>
/// <item>
///     <term>4.<see cref="Between"/></term>
///     <description>
///     Distribute items evenly. The first item is flush with the start, the last is flush with the end.
///     </description>
/// </item>
/// <item>
///     <term>5.<see cref="Arround"/></term>
///     <description>
///     Distribute items evenly. Start and end gaps are half the size of the space between each item.
///     </description>
/// </item>
/// <item>
///     <term>6.<see cref="Evenly"/></term>
///     <description>
///     Distribute items evenly. Start, in-between, and end gaps have equal sizes.
///     </description>
/// </item>
/// </list>
/// </summary>
public enum JustifyContents : byte
{
    [Description("")]
    None = 0,

    /// <summary>
    /// <c>justify-content-start</c>: Pack items from the start.
    /// </summary>
    [Description("justify-content-start")]
    Start = 1,

    /// <summary>
    /// <c>justify-content-end</c>: Pack items from the end.
    /// </summary>
    [Description("justify-content-end")]
    End = 2,

    /// <summary>
    /// <c>justify-content-center</c>: Pack items around the center.
    /// </summary>
    [Description("justify-content-center")]
    Center = 3,

    /// <summary>
    /// <c>justify-content-between</c>: Distribute items evenly. The first item is flush with the start, the last is flush with the end.
    /// </summary>
    [Description("justify-content-between")]
    Between = 4,

    /// <summary>
    /// <c>justify-content-around</c>: Distribute items evenly. Start and end gaps are half the size of the space between each item.
    /// </summary>
    [Description("justify-content-around")]
    Arround = 5,

    /// <summary>
    /// <c>justify-content-evenly</c>: Distribute items evenly. Start, in-between, and end gaps have equal sizes.
    /// </summary>
    [Description("justify-content-evenly")]
    Evenly = 6
}