using Chinook.Api.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chinook.Api.Data.EntityConfig;

internal class MediaTypeConfig : IEntityTypeConfiguration<MediaType>
{
    public void Configure(EntityTypeBuilder<MediaType> builder)
    {
        builder.ToTable("MediaType").HasKey(x => x.MediaTypeId);

        #region Properties

        builder.Property(x => x.MediaTypeId)
            .HasColumnName("MediaTypeId")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasMaxLength(120);

        #endregion Properties
    }
}
