using Domain.Interfaces;
using MediatR;

namespace Application.Features.Addresses.Queries.GetProvinces;

public class GetProvincesQueryHandler(IUnitOfWork unitOfWork, AddressMapper mapper)
    : IRequestHandler<GetProvincesQuery, IReadOnlyList<ProvinceDto>>
{
    public async Task<IReadOnlyList<ProvinceDto>>
        Handle(GetProvincesQuery request, CancellationToken cancellationToken) =>
        mapper.MapToProvinceDtoList(await unitOfWork.Provinces.GetAllReadOnlyAsync());
}