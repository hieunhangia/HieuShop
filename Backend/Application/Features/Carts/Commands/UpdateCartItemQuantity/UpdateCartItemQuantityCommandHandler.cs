using Domain.Interfaces;
using MediatR;

namespace Application.Features.Carts.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCartItemQuantityCommand>
{
    public async Task Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.CartItems.UpdateCartItemQuantityAsync(request.UserId, request.ProductVariantId,
            request.NewQuantity);
        await unitOfWork.CompleteAsync();
    }
}