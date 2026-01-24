using MediatR;

namespace Application.Features.Brands.Queries.GetBrands;

public class GetBrandsQuery : IRequest<IReadOnlyList<BrandDto>>
{
    public int? Top { get; set; }
}