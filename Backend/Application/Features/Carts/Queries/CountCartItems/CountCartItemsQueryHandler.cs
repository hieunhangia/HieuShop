using Domain.Interfaces;
using MediatR;

namespace Application.Features.Carts.Queries.CountCartItems;

public class CountCartItemsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<CountCartItemsQuery, int>
{
    public async Task<int> Handle(CountCartItemsQuery request, CancellationToken cancellationToken) =>
        await unitOfWork.CartItems.CountCartItemsAsync(request.UserId);
}