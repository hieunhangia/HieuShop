using Application.Features.Coupons.Enums;
using Domain.Enums;
using Domain.Enums.Coupons;

namespace Application.Features.Coupons.Queries.SearchUserCoupons;

public class SearchUserCouponsRequest
{
    public string? SearchText { get; set; }
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public DiscountType? DiscountType { get; set; }
    public SortDirection? SortDirection { get; set; }
    public UserCouponSortColumn? SortColumn { get; set; }
}