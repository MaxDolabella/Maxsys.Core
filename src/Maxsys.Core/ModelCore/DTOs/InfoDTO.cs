namespace Maxsys.ModelCore;

/// <summary>
/// A simple object with only Id and Description.
/// </summary>
public class InfoDTO<TKey> : IDTO
{
    /// <summary>
    /// Id
    /// </summary>
    public TKey Id { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; }
}