namespace Chinook.Api.Model.Entities;

/// <summary>
/// Table: MediaType
/// </summary>
public sealed class MediaType
{
    /// <summary>
    /// Column: MediaTypeId
    /// </summary>
    public int MediaTypeId { get; set; }

    /// <summary>
    /// Column: Name
    /// </summary>
    public string? Name { get; set; }

    public ICollection<Track> Tracks { get; set; } = [];
}
