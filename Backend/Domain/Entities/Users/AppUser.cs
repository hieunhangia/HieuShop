using Domain.Entities.Carts;
using Domain.Entities.Orders;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users;

public class AppUser : IdentityUser<Guid>
{
    public Guid? DefaultShippingAddressId { get; set; }

    public ICollection<CartItem>? CartItems { get; set; }
    public ICollection<Order>? Orders { get; set; }
    public ICollection<UserShippingAddress>? ShippingAddresses { get; set; }
    public UserShippingAddress? DefaultShippingAddress { get; set; }
}