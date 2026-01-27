using MediatR;

namespace Application.Features.Carts.Commands.RemoveCartItem;

public class RemoveCartItemCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid ProductVariantId { get; set; }
}