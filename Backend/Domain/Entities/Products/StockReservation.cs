using Domain.Commons;
using Domain.Enums.Products;

namespace Domain.Entities.Products;

public class StockReservation : BaseAuditableEntity<Guid>
{
    public required int Quantity { get; set; }
    public required DateTime ReservedAt { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public required StockReservationStatus Status { get; set; }
    public Guid ProductVariantId { get; set; }

    public ProductVariant? ProductVariant { get; set; }
}