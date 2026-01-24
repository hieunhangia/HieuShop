using Domain.Entities.Products;
using Domain.Interfaces.Repositories.Products;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Products;

public class BrandRepository(AppDbContext context) : GenericRepository<Brand, Guid>(context), IBrandRepository
{
    public async Task<IEnumerable<Brand>> QueryActiveBrandsReadOnlyAsync(int top) =>
        await Context.Brands.AsNoTracking()
            .OrderBy(b => b.DisplayOrder)
            .Take(top)
            .ToListAsync();
}