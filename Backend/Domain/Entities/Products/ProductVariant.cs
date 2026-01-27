using Domain.Common;

namespace Domain.Entities.Products;

public class ProductVariant : BaseAuditableEntity<Guid>
{
    public required long Price { get; set; }
    public required int AvailableStock { get; set; }
    public required string ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid ProductId { get; set; }

    public Product? Product { get; set; }
    public ICollection<ProductOptionValue>? ProductOptionValues { get; set; }
}