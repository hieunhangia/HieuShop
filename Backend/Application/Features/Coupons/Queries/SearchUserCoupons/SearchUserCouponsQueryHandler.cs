using Application.Common.Models;
using Application.Features.Coupons.DTOs;
using Application.Features.Coupons.Enums;
using Domain.Entities.Coupons;
using Domain.Entities.Users;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Coupons.Queries.SearchUserCoupons;

public class SearchUserCouponsQueryHandler(IUnitOfWork unitOfWork, CouponMapper mapper)
    : IRequestHandler<SearchUserCouponsQuery, PagedAndSortedResult<UserCouponDto>>
{
    public async Task<PagedAndSortedResult<UserCouponDto>>
        Handle(SearchUserCouponsQuery request, CancellationToken cancellationToken)
    {
        var pageIndex = request.PageIndex ?? 1;
        var pageSize = request.PageSize ?? 10;
        request.SortColumn ??= UserCouponSortColumn.DiscountValue;
        var sortColumn = request.SortColumn switch
        {
            UserCouponSortColumn.MaxDiscountAmount => $"{nameof(UserCoupon.Coupon)}.{nameof(Coupon.MaxDiscountAmount)}",
            UserCouponSortColumn.MinOrderAmount => $"{nameof(UserCoupon.Coupon)}.{nameof(Coupon.MinOrderAmount)}",
            UserCouponSortColumn.Used => $"{nameof(UserCoupon.Used)}",
            UserCouponSortColumn.DiscountValue => $"{nameof(UserCoupon.Coupon)}.{nameof(Coupon.DiscountValue)}",
            _ => $"{nameof(UserCoupon.CreatedAt)}"
        };
        var sortDirection = request.SortDirection ?? SortDirection.Asc;

        var pagedUserCoupons = await unitOfWork.UserCoupons.SearchUserCouponsReadOnlyAsync(request.UserId,
            request.SearchText, request.DiscountType, pageIndex, pageSize, sortColumn, sortDirection);
        return new PagedAndSortedResult<UserCouponDto>(mapper.MapToUserCouponDtoList(pagedUserCoupons.UserCoupons),
            pagedUserCoupons.TotalCount, pageIndex, pageSize, sortColumn, sortDirection);
    }
}