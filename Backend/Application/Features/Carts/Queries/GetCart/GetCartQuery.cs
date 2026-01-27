using MediatR;

namespace Application.Features.Carts.Queries.GetCart;

public class GetCartQuery : IRequest<CartDto>
{
    public Guid UserId { get; set; }
}