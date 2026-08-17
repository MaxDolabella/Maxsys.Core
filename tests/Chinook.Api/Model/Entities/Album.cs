using Chinook.Api.Model.Enums;

namespace Chinook.Api.Model.Entities;

/// <summary>
/// Table: Album
/// </summary>
public sealed class Album
{
    /// <summary>
    /// Column: AlbumId
    /// </summary>
    public int AlbumId { get; set; }

    /// <summary>
    /// Column: Title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Column: AlbumType
    /// </summary>
    public AlbumType AlbumType { get; set; }

    /// <summary>
    /// Column: ArtistId
    /// </summary>
    public int ArtistId { get; set; }

    public Artist Artist { get; set; } = null!;

    public ICollection<Track> Tracks { get; set; } = [];
}
