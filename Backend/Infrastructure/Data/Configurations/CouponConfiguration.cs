using Domain.Constants;
using Domain.Entities.Coupons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.HasIndex(c => c.Code).IsUnique();
        builder.Property(c => c.Code)
            .HasMaxLength(DataSchema.Coupon.CodeMaxLength)
            .IsUnicode(false);

        builder.Property(c => c.Name)
            .HasMaxLength(DataSchema.Coupon.NameMaxLength);

        builder.Property(c => c.Description)
            .HasMaxLength(DataSchema.Coupon.DescriptionMaxLength);
    }
}