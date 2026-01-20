using Domain.Entities.Coupons;
using Domain.Entities.Products;
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

public class CouponApplicableProductConfiguration : IEntityTypeConfiguration<CouponApplicableProduct>
{
    public void Configure(EntityTypeBuilder<CouponApplicableProduct> builder)
    {
        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(cap => cap.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class CouponApplicableCategoryConfiguration : IEntityTypeConfiguration<CouponApplicableCategory>
{
    public void Configure(EntityTypeBuilder<CouponApplicableCategory> builder)
    {
        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(cac => cac.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class CouponApplicableBrandConfiguration : IEntityTypeConfiguration<CouponApplicableBrand>
{
    public void Configure(EntityTypeBuilder<CouponApplicableBrand> builder)
    {
        builder.HasOne<Brand>()
            .WithMany()
            .HasForeignKey(cab => cab.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}