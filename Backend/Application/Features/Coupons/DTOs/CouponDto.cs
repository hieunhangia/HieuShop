namespace Application.Features.Coupons.DTOs;

public class CouponDto
{
    public required Guid Id { get; set; }
    public required string Description { get; set; }
    public required string DiscountType { get; set; }
    public required long DiscountValue { get; set; }
    public long? MaxDiscountAmount { get; set; }
    public long? MinOrderAmount { get; set; }
    public long LoyaltyPointsCost { get; set; }
}