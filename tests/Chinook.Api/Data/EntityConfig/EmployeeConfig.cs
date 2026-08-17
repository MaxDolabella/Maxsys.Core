using Chinook.Api.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chinook.Api.Data.EntityConfig;

internal class EmployeeConfig : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employee").HasKey(x => x.EmployeeId);

        #region Properties

        builder.Property(x => x.EmployeeId)
            .HasColumnName("EmployeeId")
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasColumnName("LastName")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.FirstName)
            .HasColumnName("FirstName")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasColumnName("Title")
            .HasMaxLength(30);

        builder.Property(x => x.ReportsTo)
            .HasColumnName("ReportsTo");

        builder.Property(x => x.BirthDate)
            .HasColumnName("BirthDate");

        builder.Property(x => x.HireDate)
            .HasColumnName("HireDate");

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
            .HasMaxLength(60);

        #endregion Properties

        #region Relationships

        builder.HasOne(x => x.Manager)
            .WithMany(x => x.DirectReports)
            .HasForeignKey(x => x.ReportsTo)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion Relationships
    }
}
