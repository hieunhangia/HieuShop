using Domain.Commons;

namespace Domain.Entities.Products;

public class ProductOption : BaseAuditableEntity<Guid>
{
    public required string Name { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid ProductId { get; set; }

    public ICollection<ProductOptionValue>? ProductOptionValues { get; set; }
}