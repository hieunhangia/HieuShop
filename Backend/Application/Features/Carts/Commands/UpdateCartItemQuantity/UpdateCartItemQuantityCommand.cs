using MediatR;

namespace Application.Features.Carts.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid ProductVariantId { get; set; }
    public int NewQuantity { get; set; }
}