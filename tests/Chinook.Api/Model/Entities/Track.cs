namespace Chinook.Api.Model.Entities;

/// <summary>
/// Table: Track
/// </summary>
public sealed class Track
{
    /// <summary>
    /// Column: TrackId
    /// </summary>
    public int TrackId { get; set; }

    /// <summary>
    /// Column: Name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Column: AlbumId
    /// </summary>
    public int? AlbumId { get; set; }

    /// <summary>
    /// Column: MediaTypeId
    /// </summary>
    public int MediaTypeId { get; set; }

    /// <summary>
    /// Column: GenreId
    /// </summary>
    public int? GenreId { get; set; }

    /// <summary>
    /// Column: Composer
    /// </summary>
    public string? Composer { get; set; }

    /// <summary>
    /// Column: Milliseconds
    /// </summary>
    public int Milliseconds { get; set; }

    /// <summary>
    /// Column: Bytes
    /// </summary>
    public int? Bytes { get; set; }

    /// <summary>
    /// Column: UnitPrice
    /// </summary>
    public decimal UnitPrice { get; set; }

    public Album? Album { get; set; }

    public MediaType MediaType { get; set; } = null!;

    public Genre? Genre { get; set; }

    public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = [];
}
