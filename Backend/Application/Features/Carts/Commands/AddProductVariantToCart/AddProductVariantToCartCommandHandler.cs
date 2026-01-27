using Domain.Interfaces;
using MediatR;

namespace Application.Features.Carts.Commands.AddProductVariantToCart;

public class AddProductVariantToCartCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AddProductVariantToCartCommand>
{
    public async Task Handle(AddProductVariantToCartCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.CartItems.AddProductVariantToCartAsync(request.UserId, request.ProductVariantId);
        await unitOfWork.CompleteAsync();
    }
}