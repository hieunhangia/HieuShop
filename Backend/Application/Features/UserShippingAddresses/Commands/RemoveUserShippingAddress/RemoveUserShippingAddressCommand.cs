using MediatR;

namespace Application.Features.UserShippingAddresses.Commands.RemoveUserShippingAddress;

public class RemoveUserShippingAddressCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid ShippingAddressId { get; set; }
}