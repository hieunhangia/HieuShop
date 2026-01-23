using Domain.Common;
using Domain.Entities.Products;
using Domain.Enums;
using Domain.Interfaces.Repositories.Products;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Products;

public class ProductRepository(AppDbContext dbContext) : GenericRepository<Guid, Product>(dbContext), IProductRepository
{
    public async Task<PagedResultEntity<Product>> GetAllActiveWithDefaultVariantPagedReadOnlyAsync(int pageIndex,
        int pageSize, string sortColumn, SortDirection sortDirection)
    {
        var query = dbContext.Products
            .AsNoTracking()
            .Include(x => x.DefaultProductVariant)
            .Where(p => p.IsActive)
            .OrderBy(sortColumn, sortDirection);

        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultEntity<Product>(items, totalItems, pageIndex, pageSize);
    }

    public async Task<Product?>
        GetBySlugWithDetailsAsync(string slug) =>
        await dbContext.Products
            .AsNoTracking()
            .Include(x => x.DefaultProductVariant)
            .Include(p => p.Categories)
            .Include(p => p.Brand)
            .Include(p => p.ProductOptions)!
            .ThenInclude(po => po.ProductOptionValues)
            .FirstOrDefaultAsync(p => p.Slug == slug);
}