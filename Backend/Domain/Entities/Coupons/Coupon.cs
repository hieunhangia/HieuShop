using Domain.Common;
using Domain.Enums.Coupons;

namespace Domain.Entities.Coupons;

public class Coupon : BaseAuditableEntity<Guid>
{
    public required string Description { get; set; }
    public required DiscountType DiscountType { get; set; }
    public required long DiscountValue { get; set; }
    public long? MaxDiscountAmount { get; set; }
    public long? MinOrderAmount { get; set; }
    public required long LoyaltyPointsCost { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<CouponApplicable>? CouponApplicables { get; set; }
}