using Application.Common.Exceptions;
using Domain.Entities.Carts;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Carts.Commands.AddProductVariantToCart;

public class AddProductVariantToCartCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AddProductVariantToCartCommand>
{
    public async Task Handle(AddProductVariantToCartCommand request, CancellationToken cancellationToken)
    {
        var productVariant =
            await unitOfWork.ProductVariants.GetByIdWithProductInfoReadOnlyAsync(request.ProductVariantId);
        if (productVariant == null)
        {
            throw new NotFoundException($"Không tìm thấy biến thể sản phẩm với ID: {request.ProductVariantId}");
        }

        if (!productVariant.Product!.IsActive || !productVariant.IsActive || productVariant.AvailableStock <= 0)
        {
            throw new BadRequestException("Không thể thêm sản phẩm vào giỏ hàng vì sản phẩm không khả dụng.");
        }

        var cartItem =
            await unitOfWork.CartItems.GetByUserIdAndProductVariantIdAsync(request.UserId,
                request.ProductVariantId);
        if (cartItem != null)
        {
            if (cartItem.Quantity >= productVariant.AvailableStock)
            {
                throw new BadRequestException("Không thể thêm sản phẩm vào giỏ hàng vì đã đạt đến số lượng tối đa.");
            }

            cartItem.Quantity++;
            unitOfWork.CartItems.Update(cartItem);
        }
        else
        {
            unitOfWork.CartItems.Add(new CartItem
            {
                UserId = request.UserId,
                ProductVariantId = request.ProductVariantId,
                Quantity = 1
            });
        }

        await unitOfWork.CompleteAsync();
    }
}