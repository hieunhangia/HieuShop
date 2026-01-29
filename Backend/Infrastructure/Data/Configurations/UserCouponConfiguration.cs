using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserCouponConfiguration : IEntityTypeConfiguration<UserCoupon>
{
    public void Configure(EntityTypeBuilder<UserCoupon> builder)
    {
        builder.HasOne(uc => uc.User)
            .WithMany(u => u.Coupons)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(uc => uc.Coupon)
            .WithMany()
            .HasForeignKey(uc => uc.CouponId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}