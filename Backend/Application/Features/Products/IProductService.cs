using Application.Features.Products.DTOs;
using Domain.Common;

namespace Application.Features.Products;

public interface IProductService
{
    Task<PagedResultEntity<ProductSummaryResponse>> GetActiveProductsAsync(GetProductsQuery query);
    Task<ProductDetailResponse?> GetProductBySlugAsync(string slug);
}