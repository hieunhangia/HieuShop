using MediatR;

namespace Application.Features.Coupons.Queries.GetLoyaltyPoints;

public class GetLoyaltyPointQuery : IRequest<long>
{
    public Guid UserId { get; set; }
}