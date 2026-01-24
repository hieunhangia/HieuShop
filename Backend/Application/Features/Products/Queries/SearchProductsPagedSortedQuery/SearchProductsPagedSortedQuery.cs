using Application.Common.Models;
using Application.Features.Products.Enums;
using Domain.Enums;
using MediatR;

namespace Application.Features.Products.Queries.SearchProductsPagedSortedQuery;

public class SearchProductsPagedSortedQuery : IRequest<PagedAndSortedResult<ProductDto>>
{
    public string? SearchText { get; set; }
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public ProductSortColumn? SortColumn { get; set; }
    public SortDirection? SortDirection { get; set; }
}