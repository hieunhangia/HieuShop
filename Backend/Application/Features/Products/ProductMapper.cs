using Application.Features.Products.Queries.SearchProductsPagedSorted;
using Domain.Entities.Products;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Products;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProductMapper
{
    [MapProperty(nameof(Product.DefaultProductVariant.Price), nameof(ProductSummaryDto.Price))]
    [MapProperty(nameof(Product.DefaultProductVariant.SalePrice), nameof(ProductSummaryDto.SalePrice))]
    [MapProperty(nameof(Product.DefaultProductImage.ImageUrl), nameof(ProductSummaryDto.ImageUrl))]
    public partial ProductSummaryDto MapToSummary(Product product);

    public partial IReadOnlyList<ProductSummaryDto> MapToSummaryList(IEnumerable<Product> products);
}