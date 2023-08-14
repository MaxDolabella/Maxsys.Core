using System.ComponentModel;

namespace Maxsys.Core.Filtering;

/// <summary>
/// SearchTermModes
/// <list type="bullet">
/// <item>
///     <term>1. <see cref="Any"/></term>
///     <description>Compara o termo em qualquer posição. Usa Contains() / LIKE '%termo%'.</description>
/// </item>
/// <item>
///     <term>2. <see cref="StartsWith"/></term>
///     <description>Compara o termo no início do texto. Usa StartWith() / LIKE 'termo%'.</description>
/// </item>
/// <item>
///     <term>3. <see cref="EndsWith"/></term>
///     <description>Compara o termo no fim do texto. Usa EndWith() / LIKE '%termo'.</description>
/// </item>
/// </list>
/// </summary>
public enum SearchTermModes : byte
{
    /// <summary>
    /// Compara o termo em qualquer posição. Usa Contains() / LIKE '%termo%'.
    /// </summary>
    [Description("Qualquer Posição")]
    Any = 1,

    /// <summary>
    /// Compara o termo no início do texto. Usa StartWith() / LIKE 'termo%'.
    /// </summary>
    [Description("Começa com")]
    StartsWith,

    /// <summary>
    /// Compara o termo no fim do texto. Usa EndWith() / LIKE '%termo'.
    /// </summary>
    [Description("Termina com")]
    EndsWith
}
