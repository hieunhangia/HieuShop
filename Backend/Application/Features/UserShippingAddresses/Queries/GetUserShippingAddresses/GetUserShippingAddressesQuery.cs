using Application.Features.UserShippingAddresses.DTOs;
using MediatR;

namespace Application.Features.UserShippingAddresses.Queries.GetUserShippingAddresses;

public class GetUserShippingAddressesQuery : IRequest<IReadOnlyList<UserShippingAddressSummaryDto>>
{
    public Guid UserId { get; set; }
}