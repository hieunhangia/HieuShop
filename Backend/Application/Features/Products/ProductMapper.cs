using Application.Features.Products.Queries.SearchProductsPagedSortedQuery;
using Domain.Entities.Products;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Products;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProductMapper
{
    [MapProperty(nameof(Product.DefaultProductVariant.Price), nameof(ProductDto.Price))]
    [MapProperty(nameof(Product.DefaultProductVariant.SalePrice), nameof(ProductDto.SalePrice))]
    [MapProperty(nameof(Product.DefaultProductImage.ImageUrl), nameof(ProductDto.ImageUrl))]
    public partial ProductDto MapToSummary(Product product);

    public partial IReadOnlyList<ProductDto> MapToSummaryList(IEnumerable<Product> products);
}