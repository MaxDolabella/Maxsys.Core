using System.ComponentModel;

namespace Maxsys.ModelCore.Sorting;

/// <summary>
/// Direction of the sort. Can be Ascendant or Descendant
/// <list type="bullet">
/// <item>
///     <term>1. <see cref="Ascendant"/></term>
///     <description>Ascendant.</description>
/// </item>
/// <item>
///     <term>2. <see cref="Descendant"/></term>
///     <description>Descendant.</description>
/// </item>
/// </list>
/// </summary>
public enum SortDirection : byte
{
    /// <summary>
    /// Ascendant
    /// </summary>
    [Description("Ascendant")]
    Ascendant = 1,

    /// <summary>
    /// Descendant
    /// </summary>
    [Description("Descendant")]
    Descendant = 2
}