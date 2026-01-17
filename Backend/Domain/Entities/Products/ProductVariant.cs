namespace Domain.Entities.Products;

public class ProductVariant
{
    public Guid Id { get; set; }
    public required long Price { get; set; }
    public required long SalePrice { get; set; }
    public required int AvailableStock { get; set; }
    public required string ImageUrl { get; set; }
    public Guid ProductId { get; set; }

    public ICollection<ProductOptionValue>? ProductOptionValues { get; set; }
}