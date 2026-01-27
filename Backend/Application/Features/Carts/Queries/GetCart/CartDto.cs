namespace Application.Features.Carts.Queries.GetCart;

public class CartDto
{
    public required IReadOnlyList<CartItemDto> CartItems { get; set; }
    public string? WarningMessage { get; set; }
}

public class CartItemDto
{
    public required ProductVariantDto ProductVariant { get; set; }
    public required int Quantity { get; set; }


    public class ProductVariantDto
    {
        public required Guid Id { get; set; }
        public required string ImageUrl { get; set; }
        public required long Price { get; set; }
        public required string ProductOptionValuesString { get; set; }

        public required ProductDto Product { get; set; }
    }

    public class ProductDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
    }
}