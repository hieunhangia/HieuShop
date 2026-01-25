using Application.Common.Models;
using Application.Features.Products.DTOs;
using Application.Features.Products.Enums;
using Domain.Entities.Products;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries.SearchProductsPagedSorted;

public class SearchProductsPagedSortedQueryHandler(IUnitOfWork unitOfWork, ProductMapper mapper)
    : IRequestHandler<SearchProductsPagedSortedQuery, PagedAndSortedResult<ProductSummaryDto>>
{
    public async Task<PagedAndSortedResult<ProductSummaryDto>> Handle(SearchProductsPagedSortedQuery request,
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
            await unitOfWork.Products.SearchActiveProductsReadOnlyAsync(request.SearchText, pageIndex, pageSize,
                sortColumn, sortDirection);
        return new PagedAndSortedResult<ProductSummaryDto>(mapper.MapToSummaryList(pagedProducts.Products),
            pagedProducts.TotalCount, pageIndex, pageSize, sortColumn, sortDirection);
    }
}