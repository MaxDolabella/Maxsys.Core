using Chinook.Api.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chinook.Api.Data.EntityConfig;

internal class InvoiceLineConfig : IEntityTypeConfiguration<InvoiceLine>
{
    public void Configure(EntityTypeBuilder<InvoiceLine> builder)
    {
        builder.ToTable("InvoiceLine").HasKey(x => x.InvoiceLineId);

        #region Properties

        builder.Property(x => x.InvoiceLineId)
            .HasColumnName("InvoiceLineId")
            .IsRequired();

        builder.Property(x => x.InvoiceId)
            .HasColumnName("InvoiceId")
            .IsRequired();

        builder.Property(x => x.TrackId)
            .HasColumnName("TrackId")
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasColumnName("UnitPrice")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasColumnName("Quantity")
            .IsRequired();

        #endregion Properties

        #region Relationships

        builder.HasOne(x => x.Invoice)
            .WithMany(x => x.InvoiceLines)
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Track)
            .WithMany()
            .HasForeignKey(x => x.TrackId)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion Relationships
    }
}
