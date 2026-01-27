using Application.Features.Carts.Queries.GetCart;
using Domain.Entities.Carts;
using Domain.Entities.Products;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Carts;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CartMapper
{
    public partial CartItemDto MapToCartItemDto(CartItem cartItem);

    [MapProperty(nameof(ProductVariant.ProductOptionValues),
        nameof(CartItemDto.ProductVariantDto.ProductOptionValuesString), Use = nameof(MapProductOptionValuesToString))]
    private partial CartItemDto.ProductVariantDto MapProductVariantToDto(ProductVariant variant);

    private static string MapProductOptionValuesToString(ICollection<ProductOptionValue> optionValues) =>
        string.Join(", ", optionValues.Select(ov => $"{ov.ProductOption!.Name}: {ov.Value}"));

    public partial IReadOnlyList<CartItemDto> MapToCartItemDtoList(IEnumerable<CartItem> cartItems);
}