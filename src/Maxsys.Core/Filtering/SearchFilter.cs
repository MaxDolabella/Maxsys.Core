using System.ComponentModel;

namespace Maxsys.Core.Filtering;

public struct SearchFilter
{
    public SearchFilter()
    { }

    public string Term { get; set; }
    public SearchTermModes Mode { get; set; } = SearchTermModes.Any;
}

/// <summary>
/// SearchTermModes
/// <list type="bullet">
/// <item>
///     <term>1. <see cref="Any"/></term>
///     <description>Busca o termo em qualquer posição. Usa Contains() / LIKE '%termo%'.</description>
/// </item>
/// <item>
///     <term>2. <see cref="StartWith"/></term>
///     <description>Busca o termo no início do texto. Usa StartWith() / LIKE 'termo%'.</description>
/// </item>
/// <item>
///     <term>3. <see cref="EndWith"/></term>
///     <description>Busca o termo no fim do texto. Usa EndWith() / LIKE '%termo'.</description>
/// </item>
/// </list>
/// </summary>
public enum SearchTermModes : byte
{
    /// <summary>
    /// Busca o termo em qualquer posição. Usa Contains() / LIKE '%termo%'.
    /// </summary>
    [Description("Qualquer Posição")]
    Any = 1,

    /// <summary>
    /// Busca o termo no início do texto. Usa StartWith() / LIKE 'termo%'.
    /// </summary>
    [Description("Começa com")]
    StartsWith,

    /// <summary>
    /// Busca o termo no fim do texto. Usa EndWith() / LIKE '%termo'.
    /// </summary>
    [Description("Termina com")]
    EndsWith
}