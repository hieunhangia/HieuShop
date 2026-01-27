using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Carts.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCartItemQuantityCommand>
{
    public async Task Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        if (!await unitOfWork.CartItems.IsCartItemBelongToUserAsync(request.UserId, request.CartItemId))
        {
            throw new ForbiddenAccessException("Bạn không có quyền thay đổi mục giỏ hàng này.");
        }

        await unitOfWork.CartItems.UpdateCartItemQuantityAsync(request.CartItemId, request.Quantity);
        await unitOfWork.CompleteAsync();
    }
}