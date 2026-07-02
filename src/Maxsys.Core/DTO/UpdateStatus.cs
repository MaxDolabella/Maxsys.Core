using System.ComponentModel;

namespace Maxsys.Core.DTO;

/// <summary>
/// Adaptação de <see cref="UpdateStatus"/>
///
/// <list type="bullet">
/// <item>
///     <term>0.<see cref="UpdateStatus.Loaded"/></term>
///     <description>Indica que o objeto veio de uma base de dados.</description>
/// </item>
/// <item>
///     <term>1.<see cref="UpdateStatus.Insert"/></term>
///     <description>Indica que o objeto deverá ser inserido na base de dados.</description>
/// </item>
/// <item>
///     <term>2.<see cref="UpdateStatus.Update"/></term>
///     <description>Indica que o objeto existe e deverá ser atualizado na base de dados.</description>
/// </item>
/// <item>
///     <term>3.<see cref="UpdateStatus.Delete"/></term>
///     <description>Indica que o objeto existe e deverá ser excluído na base de dados.</description>
/// </item>
/// <item>
///     <term>4.<see cref="UpdateStatus.None"/></term>
///     <description>Indica que não deverá ser tomada nenhuma atitude com o objeto.</description>
/// </item>
/// </list>
/// </summary>
public enum UpdateStatus : byte
{
    /// <summary>
    /// Indica que o objeto veio de uma base de dados
    /// </summary>
    [Description("Loaded")]
    Loaded = 0, // Do banco

    /// <summary>
    /// Indica que o objeto deverá ser inserido na base de dados
    /// </summary>
    [Description("Insert")]
    Insert,

    /// <summary>
    /// Indica que o objeto existe e deverá ser atualizado na base de dados
    /// </summary>
    [Description("Update")]
    Update,

    /// <summary>
    /// Indica que o objeto existe e deverá ser excluído na base de dados
    /// </summary>
    [Description("Delete")]
    Delete,

    /// <summary>
    /// Indica que não deverá ser tomada nenhuma atitude com o objeto
    /// </summary>
    [Description("None")]
    None, // NEW
}