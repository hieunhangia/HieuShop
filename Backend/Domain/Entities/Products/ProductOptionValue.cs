using Domain.Commons;

namespace Domain.Entities.Products;

public class ProductOptionValue : BaseAuditableEntity<Guid>
{
    public required string Value { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid ProductOptionId { get; set; }
}