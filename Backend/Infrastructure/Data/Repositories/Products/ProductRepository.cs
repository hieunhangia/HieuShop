using Domain.Entities.Products;
using Domain.Enums;
using Domain.Interfaces.Repositories.Products;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Products;

public class ProductRepository(AppDbContext context) : GenericRepository<Product, Guid>(context), IProductRepository
{
    public async Task<(IEnumerable<Product> Products, int TotalCount)> QueryActiveProductsReadOnlyAsync(
        string searchText, int pageIndex, int pageSize, string sortColumn, SortDirection sortDirection)
    {
        searchText = searchText.Trim();
        var queryable = Context.Products.AsNoTracking()
            .Include(p => p.DefaultProductImage)
            .Include(p => p.DefaultProductVariant)
            .Include(p => p.Brand)
            .Include(p => p.Categories)
            .Where(p => p.IsActive &&
                        (
                            p.Name.Contains(searchText) ||
                            p.Description.Contains(searchText) ||
                            p.Brand!.Name.Contains(searchText) ||
                            p.Categories!.Any(c => c.Name.Contains(searchText))
                        )
            )
            .OrderBy(sortColumn, sortDirection);

        return (await queryable
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
            await queryable.CountAsync());
    }

    public async Task<Product?> GetBySlugWithDetailsAsync(string slug) =>
        await Context.Products.AsNoTracking()
            .Include(x => x.ProductImages)
            .Include(x => x.DefaultProductVariant)
            .Include(p => p.Brand)
            .Include(p => p.Categories)
            .Include(p => p.ProductOptions)!
            .ThenInclude(po => po.ProductOptionValues)
            .FirstOrDefaultAsync(p => p.Slug == slug);
}