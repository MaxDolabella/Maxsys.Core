using Chinook.Api.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chinook.Api.Data.EntityConfig;

internal class TrackConfig : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.ToTable("Track").HasKey(x => x.TrackId);

        #region Properties

        builder.Property(x => x.TrackId)
            .HasColumnName("TrackId")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.AlbumId)
            .HasColumnName("AlbumId");

        builder.Property(x => x.MediaTypeId)
            .HasColumnName("MediaTypeId")
            .IsRequired();

        builder.Property(x => x.GenreId)
            .HasColumnName("GenreId");

        builder.Property(x => x.Composer)
            .HasColumnName("Composer")
            .HasMaxLength(220);

        builder.Property(x => x.Milliseconds)
            .HasColumnName("Milliseconds")
            .IsRequired();

        builder.Property(x => x.Bytes)
            .HasColumnName("Bytes");

        builder.Property(x => x.UnitPrice)
            .HasColumnName("UnitPrice")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        #endregion Properties

        #region Relationships

        builder.HasOne(x => x.Album)
            .WithMany(x => x.Tracks)
            .HasForeignKey(x => x.AlbumId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.MediaType)
            .WithMany(x => x.Tracks)
            .HasForeignKey(x => x.MediaTypeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Genre)
            .WithMany(x => x.Tracks)
            .HasForeignKey(x => x.GenreId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion Relationships
    }
}
