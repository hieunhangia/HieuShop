using Domain.Entities.Coupons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CouponApplicableConfiguration : IEntityTypeConfiguration<CouponApplicable>
{
    public void Configure(EntityTypeBuilder<CouponApplicable> builder)
    {
        builder.HasOne(ca => ca.Coupon)
            .WithMany(c => c.CouponApplicables)
            .HasForeignKey(ca => ca.CouponId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasDiscriminator<string>("ApplicableType")
            .HasValue<CouponApplicableAll>("All")
            .HasValue<CouponApplicableProduct>("Product")
            .HasValue<CouponApplicableCategory>("Category")
            .HasValue<CouponApplicableBrand>("Brand");
    }
}