using MediatR;

namespace Application.Features.Coupons.Commands.PurchaseCoupon;

public class PurchaseCouponCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid CouponId { get; set; }
}