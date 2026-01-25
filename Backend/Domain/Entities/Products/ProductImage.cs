using Domain.Common;

namespace Domain.Entities.Products;

public class ProductImage : BaseAuditableEntity<Guid>
{
    public required string ImageUrl { get; set; }
    public required int DisplayOrder { get; set; }
    public Guid ProductId { get; set; }
}