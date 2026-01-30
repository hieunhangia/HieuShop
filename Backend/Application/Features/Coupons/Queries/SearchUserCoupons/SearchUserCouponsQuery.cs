using Application.Common.Models;
using Application.Features.Coupons.DTOs;
using Application.Features.Coupons.Enums;
using Domain.Enums;
using Domain.Enums.Coupons;
using MediatR;

namespace Application.Features.Coupons.Queries.SearchUserCoupons;

public class SearchUserCouponsQuery : IRequest<PagedAndSortedResult<UserCouponDto>>
{
    public Guid UserId { get; set; }
    public string? SearchText { get; set; }
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public DiscountType? DiscountType { get; set; }
    public SortDirection? SortDirection { get; set; }
    public UserCouponSortColumn? SortColumn { get; set; }
}