using System.ComponentModel;

namespace Maxsys.Core.Sorting;

/// <summary>
/// SortDirection
/// <list type="bullet">
/// <item>
///     <term>1. <see cref="Ascending"/></term>
///     <description>Ascendente.</description>
/// </item>
/// <item>
///     <term>2. <see cref="Descending"/></term>
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
    Ascending = 1,

    /// <summary>
    /// Descendente
    /// </summary>
    [Description("Descendente")]
    Descending = 2
}