using Application.Features.UserShippingAddresses.DTOs;
using Domain.Entities.Addresses;
using Domain.Entities.Users;
using Riok.Mapperly.Abstractions;

namespace Application.Features.UserShippingAddresses;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class UserShippingAddressMapper
{
    [MapProperty(nameof(UserShippingAddress), nameof(UserShippingAddressSummaryDto.AddressString),
        Use = nameof(MapToAddressString))]
    public partial UserShippingAddressSummaryDto MapToSummaryDto(UserShippingAddress userShippingAddress);

    private static string MapToAddressString(UserShippingAddress shippingAddress) =>
        $"{shippingAddress.DetailAddress}, {shippingAddress.Ward!.Name}, {shippingAddress.Ward.Province!.Name}";

    public partial IReadOnlyList<UserShippingAddressSummaryDto> MapToSummaryDtoList(
        IEnumerable<UserShippingAddress> userShippingAddresses);

    [MapProperty(nameof(UserShippingAddress.Ward), nameof(UserShippingAddressDetailDto.ProvinceId),
        Use = nameof(MapWardToProvinceId))]
    public partial UserShippingAddressDetailDto MapToDetailDto(UserShippingAddress userShippingAddress);

    private static int MapWardToProvinceId(Ward ward) => ward.ProvinceId;
}