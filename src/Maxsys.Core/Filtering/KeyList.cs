using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Maxsys.Core.Filtering;

public class KeyList<TKey> : List<SearchKey<TKey>>// where TKey : struct
{
    public bool AnyInclude() => this.Any(k => k.Mode == SearchKeyModes.Include);

    public bool AnyExclude() => this.Any(k => k.Mode == SearchKeyModes.Exclude);

    public IEnumerable<TKey> Include => this.Where(k => k.Mode == SearchKeyModes.Include).Select(k => k.Key);
    public IEnumerable<TKey> Exclude => this.Where(k => k.Mode == SearchKeyModes.Exclude).Select(k => k.Key);
}

public struct SearchKey<TKey> //where TKey : struct
{
    public TKey Key { get; set; }
    public SearchKeyModes Mode { get; set; }
}

/// <summary>
/// Modos de busca por chave.
/// <list type="bullet">
/// <item>
///     <term>1. <see cref="Include"/></term>
///     <description>Indicado que a chave deve ser incluída na busca.</description>
/// </item>
/// <item>
///     <term>2. <see cref="Exclude"/></term>
///     <description>Indicado que a chave deve ser excluída na busca.</description>
/// </item>
/// </list>
/// </summary>
public enum SearchKeyModes : byte
{
    /// <summary>
    /// Indicado que a chave deve ser incluída na busca.
    /// </summary>
    [Description("Incluir")]
    Include = 1,

    /// <summary>
    /// Indicado que a chave deve ser excluída na busca.
    /// </summary>
    [Description("Excluir")]
    Exclude
}