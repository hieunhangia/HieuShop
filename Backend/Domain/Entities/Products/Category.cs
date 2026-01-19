using Domain.Commons;

namespace Domain.Entities.Products;

public class Category : BaseAuditableEntity<Guid>
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Product>? Products { get; set; }
}