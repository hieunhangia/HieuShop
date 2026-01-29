using Domain.Common;
using Domain.Entities.Carts;
using Domain.Entities.Orders;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users;

public class AppUser : IdentityUser<Guid>, IAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    public long LoyaltyPoints { get; set; }

    public ICollection<UserCoupon>? Coupons { get; set; }
    public ICollection<CartItem>? CartItems { get; set; }
    public ICollection<Order>? Orders { get; set; }
    public ICollection<UserShippingAddress>? ShippingAddresses { get; set; }
}