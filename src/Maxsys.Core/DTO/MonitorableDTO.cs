using System.ComponentModel;

namespace Maxsys.Core.DTO;

/// <summary>
/// Possui <see cref="UpdateStatus"/>:
/// <list type="bullet">
/// <item>
///     <term>0.<see cref="UpdateStatus.None"/></term>
///     <description>None.</description>
/// </item>
/// <item>
///     <term>1.<see cref="UpdateStatus.Insert"/></term>
///     <description>Insert.</description>
/// </item>
/// <item>
///     <term>2.<see cref="UpdateStatus.Update"/></term>
///     <description>Update.</description>
/// </item>
/// <item>
///     <term>3.<see cref="UpdateStatus.Delete"/></term>
///     <description>Delete.</description>
/// </item>
/// </list>
/// </summary>
public abstract class MonitorableDTO : IDTO
{
    public UpdateStatus UpdateStatus { get; set; } = UpdateStatus.None;
}

/// <summary>
/// UpdateStatus
/// <list type="bullet">
/// <item>
///     <term>0.<see cref="None"/></term>
///     <description>None.</description>
/// </item>
/// <item>
///     <term>1.<see cref="Insert"/></term>
///     <description>Insert.</description>
/// </item>
/// <item>
///     <term>2.<see cref="Update"/></term>
///     <description>Update.</description>
/// </item>
/// <item>
///     <term>3.<see cref="Delete"/></term>
///     <description>Delete.</description>
/// </item>
/// </list>
/// </summary>
public enum UpdateStatus : byte
{
    /// <summary>
    /// None
    /// </summary>
    [Description("None")]
    None = 0,

    /// <summary>
    /// Insert
    /// </summary>
    [Description("Insert")]
    Insert = 1,

    /// <summary>
    /// Update
    /// </summary>
    [Description("Update")]
    Update = 2,

    /// <summary>
    /// Delete
    /// </summary>
    [Description("Delete")]
    Delete = 3
}