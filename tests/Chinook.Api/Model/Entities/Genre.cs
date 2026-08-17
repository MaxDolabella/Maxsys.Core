namespace Chinook.Api.Model.Entities;

/// <summary>
/// Table: Genre
/// </summary>
public sealed class Genre
{
    /// <summary>
    /// Column: GenreId
    /// </summary>
    public int GenreId { get; set; }

    /// <summary>
    /// Column: Name
    /// </summary>
    public string? Name { get; set; }

    public ICollection<Track> Tracks { get; set; } = [];
}
