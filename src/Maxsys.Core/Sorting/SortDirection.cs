using System.ComponentModel;

namespace Maxsys.Core.Sorting;

/// <summary>
/// SortDirection
/// <list type="bullet">
/// <item>
///     <term>1. <see cref="Ascendant"/></term>
///     <description>Ascendente.</description>
/// </item>
/// <item>
///     <term>2. <see cref="Descendant"/></term>
///     <description>Descendente.</description>
/// </item>
/// </list>
/// </summary>
public enum SortDirection : byte
{
    /// <summary>
    /// Ascendente
    /// </summary>
    [Description("Ascendente")]
    Ascendant = 1,

    /// <summary>
    /// Descendente
    /// </summary>
    [Description("Descendente")]
    Descendant = 2
}