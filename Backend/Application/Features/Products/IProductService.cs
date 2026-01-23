using Application.Common.Models;
using Application.Features.Products.DTOs;

namespace Application.Features.Products;

public interface IProductService
{
    Task<PagedAndSortedResult<ProductSummaryResponse>> QueryActiveProductsAsync(GetProductsQuery query);
    Task<ProductDetailResponse?> GetProductBySlugAsync(string slug);
}