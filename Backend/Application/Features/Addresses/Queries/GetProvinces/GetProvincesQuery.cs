using MediatR;

namespace Application.Features.Addresses.Queries.GetProvinces;

public class GetProvincesQuery : IRequest<IReadOnlyList<ProvinceDto>>;