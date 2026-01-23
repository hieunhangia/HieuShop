using Domain.Common;

namespace Domain.Entities.Products;

public class ProductVariant : BaseAuditableEntity<Guid>
{
    public required long Price { get; set; }
    public long? SalePrice { get; set; }
    public required int AvailableStock { get; set; }
    public Guid ProductId { get; set; }

    public ICollection<ProductOptionValue>? ProductOptionValues { get; set; }
}