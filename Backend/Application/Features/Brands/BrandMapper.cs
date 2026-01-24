using Application.Features.Brands.Queries.GetBrands;
using Domain.Entities.Products;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Brands;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BrandMapper
{
    public partial BrandDto MapToDto(Brand brand);

    public partial IReadOnlyList<BrandDto> MapToDtoList(IEnumerable<Brand> brands);
}