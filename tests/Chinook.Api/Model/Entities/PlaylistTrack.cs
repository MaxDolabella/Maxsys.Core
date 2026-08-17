namespace Chinook.Api.Model.Entities;

/// <summary>
/// Table: PlaylistTrack
/// </summary>
public sealed class PlaylistTrack
{
    /// <summary>
    /// Column: PlaylistId
    /// </summary>
    public int PlaylistId { get; set; }

    /// <summary>
    /// Column: TrackId
    /// </summary>
    public int TrackId { get; set; }

    public Playlist Playlist { get; set; } = null!;

    public Track Track { get; set; } = null!;
}
