namespace Domain.Entities.Coupons;

public abstract class CouponApplicable
{
    public Guid Id { get; set; }
    public Guid CouponId { get; set; }

    public Coupon? Coupon { get; set; }
}