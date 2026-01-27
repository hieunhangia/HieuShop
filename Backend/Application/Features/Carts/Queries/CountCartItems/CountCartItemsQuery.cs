using MediatR;

namespace Application.Features.Carts.Queries.CountCartItems;

public class CountCartItemsQuery : IRequest<int>
{
    public Guid UserId { get; set; }
}