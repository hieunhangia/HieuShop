using Domain.Constants;
using Domain.Entities.Coupons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.Property(c => c.Description)
            .HasMaxLength(DataSchema.Coupon.DescriptionMaxLength);
    }
}