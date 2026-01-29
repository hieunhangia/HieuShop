using Domain.Common;
using Domain.Entities.Coupons;

namespace Domain.Entities.Users;

public class UserCoupon : BaseAuditableEntity<Guid>
{
    public Guid UserId { get; set; }
    public Guid CouponId { get; set; }
    public bool Used { get; set; }
    
    public AppUser? User { get; set; }
    public Coupon? Coupon { get; set; }
}