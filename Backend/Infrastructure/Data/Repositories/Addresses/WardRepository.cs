using Domain.Entities.Addresses;
using Domain.Interfaces.Repositories.Addresses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Addresses;

public class WardRepository(AppDbContext context) : GenericRepository<Ward, int>(context), IWardRepository
{
    public async Task<IReadOnlyList<Ward>> GetListByProvinceIdReadOnlyAsync(int provinceId) =>
        await Context.Wards.AsNoTracking()
            .Where(w => w.ProvinceId == provinceId)
            .ToListAsync();
}