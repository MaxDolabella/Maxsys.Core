namespace Chinook.Api.Model.Entities;

/// <summary>
/// Table: Artist
/// </summary>
public sealed class Artist
{
    /// <summary>
    /// Column: ArtistId
    /// </summary>
    public int ArtistId { get; set; }

    /// <summary>
    /// Column: Name
    /// </summary>
    public string? Name { get; set; }

    public ICollection<Album> Albums { get; set; } = [];
}
