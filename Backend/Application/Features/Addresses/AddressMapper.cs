using Application.Features.Addresses.Queries.GetProvinces;
using Application.Features.Addresses.Queries.GetWardsByProvinceId;
using Domain.Entities.Addresses;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Addresses;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AddressMapper
{
    public partial ProvinceDto MapToProvinceDto(Province province);
    public partial IReadOnlyList<ProvinceDto> MapToProvinceDtoList(IEnumerable<Province> provinces);

    public partial WardDto MapToWardDto(Ward ward);
    public partial IReadOnlyList<WardDto> MapToWardDtoList(IEnumerable<Ward> wards);
}