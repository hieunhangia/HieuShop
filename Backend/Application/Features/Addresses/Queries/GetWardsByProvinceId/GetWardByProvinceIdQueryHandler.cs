using Domain.Interfaces;
using MediatR;

namespace Application.Features.Addresses.Queries.GetWardsByProvinceId;

public class GetWardByProvinceIdQueryHandler(IUnitOfWork unitOfWork, AddressMapper mapper)
    : IRequestHandler<GetWardByProvinceIdQuery, IReadOnlyList<WardDto>>
{
    public async Task<IReadOnlyList<WardDto>> Handle(GetWardByProvinceIdQuery request,
        CancellationToken cancellationToken) =>
        mapper.MapToWardDtoList(await unitOfWork.Wards.GetListByProvinceIdReadOnlyAsync(request.ProvinceId));
}