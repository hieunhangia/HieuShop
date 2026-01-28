using MediatR;

namespace Application.Features.UserShippingAddresses.Commands.AddUserShippingAddress;

public class AddUserShippingAddressCommand : IRequest
{
    public required Guid UserId { get; set; }
    public required string RecipientName { get; set; }
    public required string RecipientPhone { get; set; }
    public required string DetailAddress { get; set; }
    public required int WardId { get; set; }
}