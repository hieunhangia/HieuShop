using Domain.Entities.Users;

namespace Domain.Interfaces.Repositories.Users;

public interface IUserCouponRepository : IGenericRepository<UserCoupon, Guid>
{
    Task<IReadOnlyList<UserCoupon>> GetListWithCouponByUserIdReadOnlyAsync(Guid userId);
}