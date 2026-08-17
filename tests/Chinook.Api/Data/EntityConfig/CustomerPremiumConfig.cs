using Chinook.Api.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chinook.Api.Data.EntityConfig;

internal class CustomerPremiumConfig : IEntityTypeConfiguration<CustomerPremium>
{
    public void Configure(EntityTypeBuilder<CustomerPremium> builder)
    {
        builder.ToTable("CustomerPremium");

        #region Properties

        builder.Property(x => x.MemberSince)
            .HasColumnName("MemberSince")
            .IsRequired();

        builder.Property(x => x.DiscountRate)
            .HasColumnName("DiscountRate")
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(x => x.MembershipTier)
            .HasColumnName("MembershipTier")
            .HasMaxLength(20);

        #endregion Properties
    }
}
