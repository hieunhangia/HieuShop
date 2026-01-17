using Domain.Entities.Products;

namespace Domain.Entities.Coupons;

public class CouponApplicableCategory : CouponApplicable
{
    public Guid CategoryId { get; set; }

    public Category? Category { get; set; }
}