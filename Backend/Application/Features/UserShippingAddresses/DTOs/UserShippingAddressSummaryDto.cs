namespace Application.Features.UserShippingAddresses.DTOs;

public class UserShippingAddressSummaryDto
{
    public required Guid Id { get; set; }
    public required string RecipientName { get; set; }
    public required string RecipientPhone { get; set; }
    public required string AddressString { get; set; }
}