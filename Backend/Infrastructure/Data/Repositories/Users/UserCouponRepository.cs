using Domain.Entities.Users;
using Domain.Enums;
using Domain.Enums.Coupons;
using Domain.Interfaces.Repositories.Users;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Users;

public class UserCouponRepository(AppDbContext context)
    : GenericRepository<UserCoupon, Guid>(context), IUserCouponRepository
{
    public async Task<(IReadOnlyList<UserCoupon> UserCoupons, int TotalCount)> SearchUserCouponsReadOnlyAsync(
        Guid userId, string? searchText, DiscountType? discountType, int pageIndex, int pageSize, string sortColumn,
        SortDirection sortDirection)
    {
        var query = Context.UserCoupons.AsNoTracking()
            .Where(uc => uc.UserId == userId);

        if (discountType != null)
        {
            query = query.Where(uc => uc.Coupon!.DiscountType == discountType);
        }

        searchText = searchText?.Trim();
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(uc => uc.Coupon!.Description.Contains(searchText));
        }

        var totalCount = await query.CountAsync();
        if (totalCount == 0)
        {
            return ([], 0);
        }

        if (sortColumn == $"{nameof(UserCoupon.Coupon)}.{nameof(UserCoupon.Coupon.MaxDiscountAmount)}")
        {
            query = sortDirection == SortDirection.Asc
                ? query.OrderBy(c => c.Coupon!.MaxDiscountAmount ?? long.MaxValue)
                : query.OrderByDescending(c => c.Coupon!.MaxDiscountAmount ?? long.MaxValue);
        }
        else
        {
            query = query.OrderBy(sortColumn, sortDirection);
        }

        return (await query
                .Include(uc => uc.Coupon)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
            totalCount);
    }
}