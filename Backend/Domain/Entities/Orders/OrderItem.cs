using Domain.Commons;
using Domain.Entities.Products;

namespace Domain.Entities.Orders;

public class OrderItem : BaseEntity<Guid>
{
    public required int Quantity { get; set; }
    public required long UnitPrice { get; set; }
    public required string ProductName { get; set; }
    public required string ProductVariantOptionSnapshot { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductVariantId { get; set; }

    public ProductVariant? ProductVariant { get; set; }
}