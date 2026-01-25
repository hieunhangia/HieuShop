using Application.Features.Products.Enums;
using Domain.Enums;

namespace Application.Features.Products.DTOs;

public class SearchProductsPagedSortedRequest
{
    public string? SearchText { get; set; }
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public ProductSortColumn? SortColumn { get; set; }
    public SortDirection? SortDirection { get; set; }
}