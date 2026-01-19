using Domain.Commons;
using Domain.Entities.Carts;
using Domain.Entities.Orders;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users;

public class AppUser : IdentityUser<Guid>, IAuditableEntity
{
    public Guid? DefaultShippingAddressId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    public ICollection<CartItem>? CartItems { get; set; }
    public ICollection<Order>? Orders { get; set; }
    public ICollection<UserShippingAddress>? ShippingAddresses { get; set; }
    public UserShippingAddress? DefaultShippingAddress { get; set; }
}