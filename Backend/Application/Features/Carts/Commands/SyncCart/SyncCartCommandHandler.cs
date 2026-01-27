using Application.Features.Carts.DTOs;
using Domain.Entities.Carts;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Carts.Commands.SyncCart;

public class SyncCartCommandHandler(IUnitOfWork unitOfWork, CartMapper mapper)
    : IRequestHandler<SyncCartCommand, CartDto>
{
    public async Task<CartDto> Handle(SyncCartCommand request,
        CancellationToken cancellationToken)
    {
        var cartItems = await unitOfWork.CartItems.GetListWithDetailAsync(request.UserId);

        var isCartUpdated = false;
        var itemsToRemove = new HashSet<CartItem>();
        foreach (var cartItem in cartItems)
        {
            if (!cartItem.ProductVariant!.Product!.IsActive || !cartItem.ProductVariant!.IsActive ||
                cartItem.ProductVariant.AvailableStock <= 0)
            {
                itemsToRemove.Add(cartItem);
                continue;
            }

            if (cartItem.Quantity <= cartItem.ProductVariant.AvailableStock) continue;
            cartItem.Quantity = cartItem.ProductVariant.AvailableStock;
            isCartUpdated = true;
        }

        if (itemsToRemove.Count != 0)
        {
            unitOfWork.CartItems.RemoveRange(itemsToRemove);
            cartItems.RemoveAll(ci => itemsToRemove.Contains(ci));
            isCartUpdated = true;
        }

        if (isCartUpdated)
        {
            await unitOfWork.CompleteAsync();
        }

        return new CartDto
        {
            CartItems = mapper.MapToCartItemDtoList(cartItems),
            WarningMessage = isCartUpdated
                ? "Giỏ hàng của bạn đã được cập nhật do một số sản phẩm đã không còn khả dụng hoặc số lượng vượt quá tồn kho."
                : null
        };
    }
}