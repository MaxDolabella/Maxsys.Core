using Chinook.Api.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chinook.Api.Data.EntityConfig;

internal class InvoiceConfig : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoice").HasKey(x => x.InvoiceId);

        #region Properties

        builder.Property(x => x.InvoiceId)
            .HasColumnName("InvoiceId")
            .IsRequired();

        builder.Property(x => x.CustomerId)
            .HasColumnName("CustomerId")
            .IsRequired();

        builder.Property(x => x.InvoiceDate)
            .HasColumnName("InvoiceDate")
            .IsRequired();

        builder.Property(x => x.BillingAddress)
            .HasColumnName("BillingAddress")
            .HasMaxLength(70);

        builder.Property(x => x.BillingCity)
            .HasColumnName("BillingCity")
            .HasMaxLength(40);

        builder.Property(x => x.BillingState)
            .HasColumnName("BillingState")
            .HasMaxLength(40);

        builder.Property(x => x.BillingCountry)
            .HasColumnName("BillingCountry")
            .HasMaxLength(40);

        builder.Property(x => x.BillingPostalCode)
            .HasColumnName("BillingPostalCode")
            .HasMaxLength(10);

        builder.Property(x => x.Total)
            .HasColumnName("Total")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        #endregion Properties

        #region Relationships

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.Invoices)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion Relationships
    }
}
