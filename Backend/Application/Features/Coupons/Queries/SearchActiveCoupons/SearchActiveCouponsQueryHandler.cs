using Application.Common.Models;
using Application.Features.Coupons.DTOs;
using Application.Features.Coupons.Enums;
using Domain.Entities.Coupons;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Coupons.Queries.SearchActiveCoupons;

public class SearchActiveCouponsQueryHandler(IUnitOfWork unitOfWork, CouponMapper mapper)
    : IRequestHandler<SearchActiveCouponsQuery, PagedAndSortedResult<CouponDto>>
{
    public async Task<PagedAndSortedResult<CouponDto>> Handle(SearchActiveCouponsQuery request,
        CancellationToken cancellationToken)
    {
        var pageIndex = request.PageIndex ?? 1;
        var pageSize = request.PageSize ?? 10;
        request.SortColumn ??= CouponSortColumn.DiscountValue;
        var sortColumn = request.SortColumn switch
        {
            CouponSortColumn.MaxDiscountAmount => nameof(Coupon.MaxDiscountAmount),
            CouponSortColumn.MinOrderAmount => nameof(Coupon.MinOrderAmount),
            CouponSortColumn.LoyaltyPointsCost => nameof(Coupon.LoyaltyPointsCost),
            _ => nameof(Coupon.DiscountValue)
        };
        var sortDirection = request.SortDirection ?? SortDirection.Asc;

        var pagedCoupons = await unitOfWork.Coupons.SearchActiveCouponsReadOnlyAsync(request.SearchText,
            request.DiscountType, pageIndex, pageSize, sortColumn, sortDirection);
        return new PagedAndSortedResult<CouponDto>(mapper.MapToDtoList(pagedCoupons.Coupons), pagedCoupons.TotalCount,
            pageIndex, pageSize, sortColumn, sortDirection);
    }
}