using Domain.Interfaces;
using MediatR;

namespace Application.Features.Brands.Queries.GetBrands;

public class GetBrandsQueryHandler(IUnitOfWork unitOfWork, BrandMapper mapper)
    : IRequestHandler<GetBrandsQuery, IReadOnlyList<BrandDto>>
{
    public async Task<IReadOnlyList<BrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken) =>
        mapper.MapToDtoList(await unitOfWork.Brands.GetTopActiveBrandsReadOnlyAsync(request.Top ?? 10));
}