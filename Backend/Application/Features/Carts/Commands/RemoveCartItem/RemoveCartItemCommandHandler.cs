using Domain.Interfaces;
using MediatR;

namespace Application.Features.Carts.Commands.RemoveCartItem;

public class RemoveCartItemCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RemoveCartItemCommand>
{
    public async Task Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.CartItems.RemoveCartItemAsync(request.UserId, request.ProductVariantId);
        await unitOfWork.CompleteAsync();
    }
}