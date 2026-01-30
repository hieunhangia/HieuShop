using Application.Features.Coupons.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Coupons.Queries.GetUserCoupons;

public class GetUserCouponsQueryHandler(IUnitOfWork unitOfWork, CouponMapper mapper)
    : IRequestHandler<GetUserCouponsQuery, IReadOnlyList<UserCouponDto>>
{
    public async Task<IReadOnlyList<UserCouponDto>>
        Handle(GetUserCouponsQuery request, CancellationToken cancellationToken) =>
        mapper.MapToUserCouponDtoList(await unitOfWork.UserCoupons.GetListWithCouponByUserIdReadOnlyAsync(request.UserId));
}