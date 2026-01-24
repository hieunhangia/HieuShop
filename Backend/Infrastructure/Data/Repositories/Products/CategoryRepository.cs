using Domain.Entities.Products;
using Domain.Interfaces.Repositories.Products;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Products;

public class CategoryRepository(AppDbContext context) : GenericRepository<Category, Guid>(context), ICategoryRepository
{
    public async Task<IEnumerable<Category>> QueryActiveCategoriesReadOnlyAsync(int top) =>
        await Context.Categories.AsNoTracking()
            .OrderBy(c => c.DisplayOrder)
            .Take(top)
            .ToListAsync();
}