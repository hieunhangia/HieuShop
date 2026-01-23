using Domain.Entities.Products;
using Domain.Interfaces.Repositories.Products;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Products;

public class CategoryRepository(AppDbContext context) : GenericRepository<Category, Guid>(context), ICategoryRepository
{
    public async Task<IEnumerable<Category>> QueryActiveCategoriesReadOnlyAsync(string searchText, int top) =>
        await Context.Categories.AsNoTracking()
            .Where(c => c.IsActive && c.Name.Contains(searchText))
            .Take(top)
            .ToListAsync();
}