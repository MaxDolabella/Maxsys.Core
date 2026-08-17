using Chinook.Api.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chinook.Api.Data.EntityConfig;

internal class ArtistConfig : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("Artist").HasKey(x => x.ArtistId);

        builder.Property(x => x.ArtistId).HasColumnName("ArtistId").IsRequired();
        builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(120);
    }
}
