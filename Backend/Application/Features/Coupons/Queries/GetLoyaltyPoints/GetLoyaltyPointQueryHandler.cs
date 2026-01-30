using Domain.Interfaces;
using MediatR;
using UnauthorizedAccessException = Application.Common.Exceptions.UnauthorizedAccessException;

namespace Application.Features.Coupons.Queries.GetLoyaltyPoints;

public class GetLoyaltyPointQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetLoyaltyPointQuery, long>
{
    public async Task<long> Handle(GetLoyaltyPointQuery request, CancellationToken cancellationToken) =>
        (await unitOfWork.Users.GetByIdAsync(request.UserId))?.LoyaltyPoints ??
        throw new UnauthorizedAccessException("Bạn phải đăng nhập để thực hiện hành động này.");
}