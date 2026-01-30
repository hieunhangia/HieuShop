using Application.Common.Models;
using Application.Features.Coupons.DTOs;
using Application.Features.Coupons.Enums;
using Domain.Enums;
using Domain.Enums.Coupons;
using MediatR;

namespace Application.Features.Coupons.Queries.SearchActiveCoupons;

public class SearchActiveCouponsQuery : IRequest<PagedAndSortedResult<CouponDto>>
{
    public string? SearchText { get; set; }
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public DiscountType? DiscountType { get; set; }
    public SortDirection? SortDirection { get; set; }
    public CouponSortColumn? SortColumn { get; set; }
}