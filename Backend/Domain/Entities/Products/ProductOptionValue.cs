namespace Domain.Entities.Products;

public class ProductOptionValue
{
    public Guid Id { get; set; }
    public required string Value { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid ProductOptionId { get; set; }
}