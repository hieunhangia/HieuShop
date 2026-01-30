namespace Application.Features.Coupons.DTOs;

public class UserCouponDto
{
    public required Guid Id { get; set; }
    public required CouponDto Coupon { get; set; }
    public required bool Used { get; set; }
    public required DateTime CreatedAt { get; set; }
}