using Chinook.Api.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chinook.Api.Data.EntityConfig;

internal class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.UseTptMappingStrategy();
        builder.ToTable("Customer").HasKey(x => x.CustomerId);

        #region Properties

        builder.Property(x => x.CustomerId)
            .HasColumnName("CustomerId")
            .IsRequired();

        builder.Property(x => x.FirstName)
            .HasColumnName("FirstName")
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasColumnName("LastName")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Company)
            .HasColumnName("Company")
            .HasMaxLength(80);

        builder.Property(x => x.Address)
            .HasColumnName("Address")
            .HasMaxLength(70);

        builder.Property(x => x.City)
            .HasColumnName("City")
            .HasMaxLength(40);

        builder.Property(x => x.State)
            .HasColumnName("State")
            .HasMaxLength(40);

        builder.Property(x => x.Country)
            .HasColumnName("Country")
            .HasMaxLength(40);

        builder.Property(x => x.PostalCode)
            .HasColumnName("PostalCode")
            .HasMaxLength(10);

        builder.Property(x => x.Phone)
            .HasColumnName("Phone")
            .HasMaxLength(24);

        builder.Property(x => x.Fax)
            .HasColumnName("Fax")
            .HasMaxLength(24);

        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(x => x.SupportRepId)
            .HasColumnName("SupportRepId");

        #endregion Properties

        #region Relationships

        builder.HasOne(x => x.SupportRep)
            .WithMany(x => x.Customers)
            .HasForeignKey(x => x.SupportRepId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion Relationships
    }
}
