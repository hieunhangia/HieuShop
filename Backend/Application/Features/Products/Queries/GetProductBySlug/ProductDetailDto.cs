namespace Application.Features.Products.Queries.GetProductBySlug;

public class ProductDetailDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string Description { get; set; }
    public BrandDto? Brand { get; set; }
    public required List<CategoryDto> Categories { get; set; }
    public required List<ProductOptionDto> ProductOptions { get; set; }
    public required List<ProductVariantDto> ProductVariants { get; set; }
    public required List<ProductImageDto> ProductImages { get; set; }

    public class BrandDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public required string LogoUrl { get; set; }
    }

    public class CategoryDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public required string ImageUrl { get; set; }
    }

    public class ProductOptionDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required List<ProductOptionValueDto> ProductOptionValues { get; set; }
    }

    public class ProductOptionValueDto
    {
        public required Guid Id { get; set; }
        public required string Value { get; set; }
    }

    public class ProductVariantDto
    {
        public required Guid Id { get; set; }
        public required long Price { get; set; }
        public required int AvailableStock { get; set; }
        public required string ImageUrl { get; set; }
        public required List<ProductOptionValueDto> ProductOptionValues { get; set; }
    }

    public class ProductImageDto
    {
        public required Guid Id { get; set; }
        public required string ImageUrl { get; set; }
        public required int DisplayOrder { get; set; }
    }
}