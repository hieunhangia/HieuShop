using Application.Common.Models;
using Application.Features.Products.DTOs;
using Domain.Entities.Products;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Products;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProductMapper
{
    [MapProperty(nameof(Product.DefaultProductVariant.Price), nameof(ProductSummaryResponse.Price))]
    [MapProperty(nameof(Product.DefaultProductVariant.SalePrice), nameof(ProductSummaryResponse.SalePrice))]
    [MapProperty(nameof(Product.DefaultProductImage.ImageUrl), nameof(ProductSummaryResponse.ImageUrl))]
    public partial ProductSummaryResponse MapToSummary(Product product);

    public partial IReadOnlyList<ProductSummaryResponse> MapToSummaryList(IEnumerable<Product> products);
}