// ReSharper disable UnusedAutoPropertyAccessor.Global

using Application.Features.Products.Enums;
using Domain.Enums;

namespace Application.Features.Products.DTOs;

public class GetProductsQuery
{
    public string SearchText { get; set; } = string.Empty;
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public ProductSortColumn SortColumn { get; set; } = ProductSortColumn.CreatedAt;
    public SortDirection SortDirection { get; set; } = SortDirection.Asc;
}