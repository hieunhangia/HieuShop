using Domain.Entities.Coupons;
using Domain.Enums;
using Domain.Enums.Coupons;
using Domain.Interfaces.Repositories.Coupons;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Coupons;

public class CouponRepository(AppDbContext context) : GenericRepository<Coupon, Guid>(context), ICouponRepository
{
    public async Task<(IReadOnlyList<Coupon> Coupons, int TotalCount)> SearchActiveCouponsReadOnlyAsync(
        string? searchText, DiscountType? discountType, int pageIndex, int pageSize, string sortColumn,
        SortDirection sortDirection)
    {
        var query = Context.Coupons.AsNoTracking()
            .Where(c => c.IsActive);

        if (discountType != null)
        {
            query = query.Where(c => c.DiscountType == discountType);
        }

        searchText = searchText?.Trim();
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(c => c.Description.Contains(searchText));
        }

        var totalCount = await query.CountAsync();
        if (totalCount == 0)
        {
            return ([], 0);
        }

        return (await query
                .OrderBy(sortColumn, sortDirection)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
            totalCount);
    }
}