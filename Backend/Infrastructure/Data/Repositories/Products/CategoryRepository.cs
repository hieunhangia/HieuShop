using Domain.Entities.Products;
using Domain.Interfaces.Repositories.Products;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Products;

public class CategoryRepository(AppDbContext context) : GenericRepository<Category, Guid>(context), ICategoryRepository
{
    public async Task<IReadOnlyList<Category>> GetTopActiveCategoriesReadOnlyAsync(int top) =>
        await Context.Categories.AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .Take(top)
            .ToListAsync();

    public async Task<Category?> GetBySlugAsync(string slug) =>
        await Context.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
}