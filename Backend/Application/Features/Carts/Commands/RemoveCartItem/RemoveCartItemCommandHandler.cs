using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Carts.Commands.RemoveCartItem;

public class RemoveCartItemCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RemoveCartItemCommand>
{
    public async Task Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        var cartItem = await unitOfWork.CartItems.GetByIdAsync(request.CartItemId);
        if (cartItem != null)
        {
            if (cartItem.UserId != request.UserId)
            {
                throw new ForbiddenAccessException("Bạn không có quyền xóa mục giỏ hàng này.");
            }

            unitOfWork.CartItems.Remove(cartItem);
            await unitOfWork.CompleteAsync();
        }
    }
}