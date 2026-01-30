using Application.Features.Coupons.DTOs;
using Domain.Entities.Coupons;
using Domain.Entities.Users;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Coupons;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CouponMapper
{
    public partial CouponDto MapToDto(Coupon coupon);
    public partial IReadOnlyList<CouponDto> MapToDtoList(IEnumerable<Coupon> coupons);

    public partial UserCouponDto MapToUserCouponDto(UserCoupon userCoupon);
    public partial IReadOnlyList<UserCouponDto> MapToUserCouponDtoList(IEnumerable<UserCoupon> userCoupons);
}