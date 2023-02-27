using System.ComponentModel;

namespace Maxsys.ModelCore.Filtering;

/// <summary>
/// Used in filtering to define which objects will be retrieve.
/// <list type="bullet">
/// <item>
///     <term>0. <see cref="OnlyActives"/></term>
///     <description>Only Actives.</description>
/// </item>
/// <item>
///     <term>1. <see cref="OnlyInactives"/></term>
///     <description>Only Inactives.</description>
/// </item>
/// <item>
///     <term>2. <see cref="All"/></term>
///     <description>All.</description>
/// </item>
/// </list>
/// </summary>
public enum ActiveTypes : byte
{
    /// <summary>
    /// Only Actives
    /// </summary>
    [Description("Only Actives")]
    OnlyActives = 0,

    /// <summary>
    /// Only Inactives
    /// </summary>
    [Description("Only Inactives")]
    OnlyInactives = 1,

    /// <summary>
    /// All
    /// </summary>
    [Description("All")]
    All
}