namespace Application.Features.Products.Queries.GetProductBySlug;

public class ProductDetailDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string Description { get; set; }
    public required long Price { get; set; }
    public long? SalePrice { get; set; }
    public BrandResponse? Brand { get; set; }
    public required List<CategoryResponse> Categories { get; set; }
    public required List<ProductOptionResponse> Options { get; set; }
    public required List<string> ImageUrls { get; set; }
}

public class BrandResponse
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
}

public class CategoryResponse
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
}

public class ProductOptionResponse
{
    public required string Name { get; set; }
    public required List<ProductOptionValueResponse> Values { get; set; }
}

public class ProductOptionValueResponse
{
    public required string Value { get; set; }
}