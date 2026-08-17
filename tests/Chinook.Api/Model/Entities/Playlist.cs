namespace Chinook.Api.Model.Entities;

/// <summary>
/// Table: Playlist
/// </summary>
public sealed class Playlist
{
    /// <summary>
    /// Column: PlaylistId
    /// </summary>
    public int PlaylistId { get; set; }

    /// <summary>
    /// Column: Name
    /// </summary>
    public string? Name { get; set; }

    public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = [];
}
