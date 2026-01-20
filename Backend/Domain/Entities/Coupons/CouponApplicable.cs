using Domain.Common;

namespace Domain.Entities.Coupons;

public abstract class CouponApplicable : BaseAuditableEntity<Guid>
{
    public Guid CouponId { get; set; }

    public Coupon? Coupon { get; set; }
}