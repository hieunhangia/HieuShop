using Domain.Commons;
using Domain.Enums.Coupons;

namespace Domain.Entities.Coupons;

public class Coupon : BaseAuditableEntity<Guid>
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public required DiscountType DiscountType { get; set; }
    public required long DiscountValue { get; set; }
    public required long? MaxDiscountAmount { get; set; }
    public required long MinOrderAmount { get; set; }
    public required int UsageLimit { get; set; }
    public required int UsageCount { get; set; }
    public required int PerUserLimit { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<CouponApplicable>? CouponApplicables { get; set; }
}