namespace Application.Features.UserShippingAddresses.DTOs;

public class UserShippingAddressDetailDto
{
    public required Guid Id { get; set; }
    public required string RecipientName { get; set; }
    public required string RecipientPhone { get; set; }
    public required string DetailAddress { get; set; }
    public required int WardId { get; set; }
    public required int ProvinceId { get; set; }
}