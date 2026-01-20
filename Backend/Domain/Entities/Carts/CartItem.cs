using Domain.Common;
using Domain.Entities.Products;
using Domain.Entities.Users;

namespace Domain.Entities.Carts;

public class CartItem : BaseAuditableEntity<Guid>
{
    public int Quantity { get; set; }
    public Guid UserId { get; set; }
    public Guid ProductVariantId { get; set; }

    public AppUser? User { get; set; }
    public ProductVariant? ProductVariant { get; set; }
}