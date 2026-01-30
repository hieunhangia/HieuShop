using Domain.Entities.Users;
using Domain.Interfaces.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Users;

public class UserCouponRepository(AppDbContext context)
    : GenericRepository<UserCoupon, Guid>(context), IUserCouponRepository
{
    public async Task<IReadOnlyList<UserCoupon>> GetListWithCouponByUserIdReadOnlyAsync(Guid userId) =>
        await Context.UserCoupons.AsNoTracking()
            .Include(uc => uc.Coupon)
            .Where(uc => uc.UserId == userId)
            .ToListAsync();
}