using System.ComponentModel;

namespace Maxsys.Core.Filtering;

/// <summary>
/// SomeEnum
/// <list type="bullet">
/// <item>
///     <term>0. <see cref="OnlyActives"/></term>
///     <description>Somente ativos.</description>
/// </item>
/// <item>
///     <term>1. <see cref="OnlyInactives"/></term>
///     <description>Somente inativos.</description>
/// </item>
/// <item>
///     <term>2. <see cref="All"/></term>
///     <description>Ativos e inativos.</description>
/// </item>
/// </list>
/// </summary>
public enum ActiveTypes : byte
{
    /// <summary>
    /// Somente ativos
    /// </summary>
    OnlyActives = 0,

    /// <summary>
    /// Somente inativos
    /// </summary>
    OnlyInactives,

    /// <summary>
    /// Ativos e inativos
    /// </summary>
    All
}