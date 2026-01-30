using Domain.Entities.Users;
using Domain.Enums;
using Domain.Enums.Coupons;

namespace Domain.Interfaces.Repositories.Users;

public interface IUserCouponRepository : IGenericRepository<UserCoupon, Guid>
{
    Task<(IReadOnlyList<UserCoupon> UserCoupons, int TotalCount)> SearchUserCouponsReadOnlyAsync(Guid userId,
        string? searchText, DiscountType? discountType, int pageIndex, int pageSize, string sortColumn,
        SortDirection sortDirection);
}