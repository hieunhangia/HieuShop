using Domain.Entities.Products;

namespace Domain.Entities.Coupons;

public class CouponApplicableProduct : CouponApplicable
{
    public Guid ProductId { get; set; }

    public Product? Product { get; set; }
}