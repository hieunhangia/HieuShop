using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Carts.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCartItemQuantityCommand>
{
    public async Task Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var cartItem = await unitOfWork.CartItems.GetByIdWithProductInfoAsync(request.CartItemId);
        if (cartItem == null)
        {
            throw new NotFoundException("Mục giỏ hàng không tồn tại.");
        }

        if (cartItem.UserId != request.UserId)
        {
            throw new ForbiddenAccessException("Bạn không có quyền thay đổi mục giỏ hàng này.");
        }

        if (!cartItem.ProductVariant!.Product!.IsActive || !cartItem.ProductVariant.IsActive ||
            cartItem.ProductVariant.AvailableStock <= 0)
        {
            throw new BadRequestException("Không thể cập nhật số lượng sản phẩm vì sản phẩm không khả dụng.");
        }

        if (request.Quantity > cartItem.ProductVariant.AvailableStock)
        {
            throw new BadRequestException("Không thể cập nhật số lượng sản phẩm vì vượt quá số lượng tồn kho.");
        }

        cartItem.Quantity = request.Quantity;
        unitOfWork.CartItems.Update(cartItem);
        await unitOfWork.CompleteAsync();
    }
}