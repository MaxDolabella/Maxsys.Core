namespace Maxsys.Core;

/// <summary>
/// A simple object with only Key and Description.
/// </summary>
public class InfoDTO<TKey> : IDTO
{
    /// <summary>
    /// Key
    /// </summary>
    public TKey Key { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; }
}