using MediatR;

namespace Application.Features.UserShippingAddresses.Commands.UpdateUserShippingAddress;

public class UpdateUserShippingAddressCommand : IRequest
{
    public required Guid UserId { get; set; }
    public required Guid ShippingAddressId { get; set; }
    public required string RecipientName { get; set; }
    public required string RecipientPhone { get; set; }
    public required string DetailAddress { get; set; }
    public required int WardId { get; set; }
}