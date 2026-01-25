namespace Application.Features.Products.DTOs;

public class ProductSummaryDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required long MinPrice { get; set; }
    public required long MaxPrice { get; set; }
    public required string MainImageUrl { get; set; }
}