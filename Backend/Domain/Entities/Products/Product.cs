using Domain.Common;

namespace Domain.Entities.Products;

public class Product : BaseAuditableEntity<Guid>
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Slug { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid BrandId { get; set; }
    public required string MainImageUrl { get; set; }
    public required long MinPrice { get; set; }
    public required long MaxPrice { get; set; }

    public ICollection<ProductImage>? ProductImages { get; set; }
    public Brand? Brand { get; set; }
    public ICollection<Category>? Categories { get; set; }
    public ICollection<ProductVariant>? ProductVariants { get; set; }
    public ICollection<ProductOption>? ProductOptions { get; set; }
}