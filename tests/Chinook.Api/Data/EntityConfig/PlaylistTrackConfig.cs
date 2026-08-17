using Chinook.Api.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chinook.Api.Data.EntityConfig;

internal class PlaylistTrackConfig : IEntityTypeConfiguration<PlaylistTrack>
{
    public void Configure(EntityTypeBuilder<PlaylistTrack> builder)
    {
        builder.ToTable("PlaylistTrack").HasKey(x => new { x.PlaylistId, x.TrackId });

        #region Properties

        builder.Property(x => x.PlaylistId)
            .HasColumnName("PlaylistId")
            .IsRequired();

        builder.Property(x => x.TrackId)
            .HasColumnName("TrackId")
            .IsRequired();

        #endregion Properties

        #region Relationships

        builder.HasOne(x => x.Playlist)
            .WithMany(x => x.PlaylistTracks)
            .HasForeignKey(x => x.PlaylistId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Track)
            .WithMany(x => x.PlaylistTracks)
            .HasForeignKey(x => x.TrackId)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion Relationships
    }
}
