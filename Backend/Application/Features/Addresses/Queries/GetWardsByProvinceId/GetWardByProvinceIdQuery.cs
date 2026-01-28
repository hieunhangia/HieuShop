using MediatR;

namespace Application.Features.Addresses.Queries.GetWardsByProvinceId;

public class GetWardByProvinceIdQuery : IRequest<IReadOnlyList<WardDto>>
{
    public int ProvinceId { get; set; }
}