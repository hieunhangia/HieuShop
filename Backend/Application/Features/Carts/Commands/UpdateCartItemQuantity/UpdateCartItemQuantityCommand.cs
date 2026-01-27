using MediatR;

namespace Application.Features.Carts.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid CartItemId { get; set; }
    public int Quantity { get; set; }
}