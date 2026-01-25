using Application.Common.Models;
using Application.Features.Products.DTOs;
using Application.Features.Products.Enums;
using Domain.Entities.Products;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries.SearchProductsPagedSortedBySlug;

public class SearchProductsPagedSortedBySlugQueryHandler(IUnitOfWork unitOfWork, ProductMapper mapper)
    : IRequestHandler<SearchProductsPagedSortedBySlugQuery, PagedAndSortedResult<ProductSummaryDto>>
{
    public async Task<PagedAndSortedResult<ProductSummaryDto>> Handle(SearchProductsPagedSortedBySlugQuery request,
        CancellationToken cancellationToken)
    {
        var pageIndex = request.PageIndex ?? 1;
        var pageSize = request.PageSize ?? 10;
        request.SortColumn ??= ProductSortColumn.CreatedAt;
        var sortColumn = request.SortColumn switch
        {
            ProductSortColumn.Name => nameof(Product.Name),
            ProductSortColumn.Price =>
                $"{nameof(Product.DefaultProductVariant)}.{nameof(Product.DefaultProductVariant.Price)}",
            _ => nameof(Product.CreatedAt)
        };
        var sortDirection = request.SortDirection ?? SortDirection.Asc;

        var pagedProducts =
            await unitOfWork.Products.SearchActiveProductsBySlugReadOnlyAsync(request.Slug ?? string.Empty,
                request.SearchText, pageIndex, pageSize, sortColumn, sortDirection);
        return new PagedAndSortedResult<ProductSummaryDto>(mapper.MapToSummaryList(pagedProducts.Products),
            pagedProducts.TotalCount, pageIndex, pageSize, sortColumn, sortDirection);
    }
}