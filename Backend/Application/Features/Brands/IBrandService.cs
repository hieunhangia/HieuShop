using Application.Features.Brands.DTOs;

namespace Application.Features.Brands;

public interface IBrandService
{
    Task<IEnumerable<BrandResponse>> QueryBrandsAsync(GetBrandsQuery query);
}