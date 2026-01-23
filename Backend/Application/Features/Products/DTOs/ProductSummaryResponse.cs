namespace Application.Features.Products.DTOs;

public class ProductSummaryResponse
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required long Price { get; set; }
    public long? SalePrice { get; set; }
}