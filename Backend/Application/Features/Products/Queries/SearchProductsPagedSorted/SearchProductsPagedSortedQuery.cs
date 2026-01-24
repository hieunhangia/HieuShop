using Application.Common.Models;
using Application.Features.Products.Enums;
using Domain.Enums;
using MediatR;

namespace Application.Features.Products.Queries.SearchProductsPagedSorted;

public class SearchProductsPagedSortedQuery : IRequest<PagedAndSortedResult<ProductSummaryDto>>
{
    public string? SearchText { get; set; }
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public ProductSortColumn? SortColumn { get; set; }
    public SortDirection? SortDirection { get; set; }
}