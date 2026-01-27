using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Carts.Commands.RemoveCartItem;

public class RemoveCartItemCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RemoveCartItemCommand>
{
    public async Task Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        if (!await unitOfWork.CartItems.IsCartItemBelongToUserAsync(request.UserId, request.CartItemId))
        {
            throw new ForbiddenAccessException("Bạn không có quyền xóa mục giỏ hàng này.");
        }

        await unitOfWork.CartItems.RemoveCartItemAsync(request.CartItemId);
        await unitOfWork.CompleteAsync();
    }
}