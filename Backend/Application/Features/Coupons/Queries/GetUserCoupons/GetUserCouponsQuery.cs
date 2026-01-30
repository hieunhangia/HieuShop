using Application.Features.Coupons.DTOs;
using MediatR;

namespace Application.Features.Coupons.Queries.GetUserCoupons;

public class GetUserCouponsQuery : IRequest<IReadOnlyList<UserCouponDto>>
{
    public Guid UserId { get; set; }
}