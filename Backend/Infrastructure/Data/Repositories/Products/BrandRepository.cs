using Domain.Entities.Products;
using Domain.Interfaces.Repositories.Products;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Products;

public class BrandRepository(AppDbContext context) : GenericRepository<Brand, Guid>(context), IBrandRepository
{
    public async Task<IEnumerable<Brand>> QueryActiveBrandsReadOnlyAsync(string searchText, int top) =>
        await Context.Brands.AsNoTracking()
            .Where(b => b.IsActive && b.Name.Contains(searchText))
            .Take(top)
            .ToListAsync();
}