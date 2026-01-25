using Application.Features.Products.DTOs;
using Application.Features.Products.Queries.GetProductBySlug;
using Domain.Entities.Products;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Products;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProductMapper
{
    public partial ProductSummaryDto MapToSummary(Product product);

    public partial IReadOnlyList<ProductSummaryDto> MapToSummaryList(IEnumerable<Product> products);

    public partial ProductDetailDto MapToDetail(Product product);
}