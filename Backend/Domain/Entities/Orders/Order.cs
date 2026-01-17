using Domain.Entities.Coupons;
using Domain.Entities.Users;
using Domain.Enums.Orders;

namespace Domain.Entities.Orders;

public class Order
{
    public Guid Id { get; set; }
    public required string OrderCode { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required OrderStatus Status { get; set; }
    public required long SubTotal { get; set; }
    public required long ShippingFee { get; set; }
    public long DiscountAmount { get; set; }
    public required long FinalTotal { get; set; }
    public required string ShippingAddressSnapshot { get; set; }
    public Guid PaymentMethodId { get; set; }
    public Guid UserId { get; set; }
    public Guid? CouponId { get; set; }

    public PaymentMethod? PaymentMethod { get; set; }
    public AppUser? User { get; set; }
    public List<OrderItem>? OrderItems { get; set; }
    public Coupon? Coupon { get; set; }
}