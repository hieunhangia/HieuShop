using Domain.Entities.Addresses;

namespace Domain.Entities.Users;

public class UserShippingAddress
{
    public Guid Id { get; set; }
    public required string RecipientName { get; set; }
    public required string RecipientPhone { get; set; }
    public required string DetailAddress { get; set; }
    public Guid UserId { get; set; }
    public int WardId { get; set; }

    public AppUser? User { get; set; }
    public Ward? Ward { get; set; }
}