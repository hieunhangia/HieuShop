using Domain.Interfaces;
using MediatR;

namespace Application.Features.Carts.Queries.GetCart;

public class GetCartQueryHandler(IUnitOfWork unitOfWork, CartMapper mapper)
    : IRequestHandler<GetCartQuery, CartDto>
{
    public async Task<CartDto> Handle(GetCartQuery request,
        CancellationToken cancellationToken)
    {
        var cart = await unitOfWork.CartItems.GetCartAsync(request.UserId);
        return new CartDto
        {
            CartItems = mapper.MapToCartItemDtoList(cart.CartItems),
            WarningMessage = cart.WarningMessage
        };
    }
}