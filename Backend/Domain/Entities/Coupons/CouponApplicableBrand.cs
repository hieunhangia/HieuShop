using Domain.Entities.Products;

namespace Domain.Entities.Coupons;

public class CouponApplicableBrand : CouponApplicable
{
    public Guid BrandId { get; set; }

    public Brand? Brand { get; set; }
}