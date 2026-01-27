using MediatR;

namespace Application.Features.Carts.Commands.AddProductVariantToCart;

public class AddProductVariantToCartCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid ProductVariantId { get; set; }
}