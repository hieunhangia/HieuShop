using Application.Features.UserShippingAddresses.DTOs;
using MediatR;

namespace Application.Features.UserShippingAddresses.Queries.GetUserShippingAddress;

public class GetUserShippingAddressQuery : IRequest<UserShippingAddressDetailDto>
{
    public Guid UserId { get; set; }
    public Guid ShippingAddressId { get; set; }
}