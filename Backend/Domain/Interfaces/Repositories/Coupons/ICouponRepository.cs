using Domain.Entities.Coupons;
using Domain.Enums;
using Domain.Enums.Coupons;

namespace Domain.Interfaces.Repositories.Coupons;

public interface ICouponRepository : IGenericRepository<Coupon, Guid>
{
    Task<(IReadOnlyList<Coupon> Coupons, int TotalCount)> SearchActiveCouponsReadOnlyAsync(string? searchText,
        DiscountType? discountType, int pageIndex, int pageSize, string sortColumn, SortDirection sortDirection);
}