using Chinook.Api.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chinook.Api.Data.EntityConfig;

internal class AlbumConfig : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable("Album").HasKey(x => x.AlbumId);

        #region Properties

        builder.Property(x => x.AlbumId)
            .HasColumnName("AlbumId")
            .IsRequired();

        builder.Property(x => x.Title)
            .HasColumnName("Title")
            .HasMaxLength(160)
            .IsRequired();

        builder.Property(x => x.AlbumType)
            .HasColumnName("AlbumType")
            .HasConversion<byte>()
            .IsRequired();

        builder.Property(x => x.ArtistId)
            .HasColumnName("ArtistId")
            .IsRequired();

        #endregion Properties

        #region Relationships

        builder.HasOne(x => x.Artist)
            .WithMany(x => x.Albums)
            .HasForeignKey(x => x.ArtistId)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion Relationships
    }
}
