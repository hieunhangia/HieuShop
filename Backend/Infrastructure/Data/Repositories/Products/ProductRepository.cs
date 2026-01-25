using Domain.Entities.Products;
using Domain.Enums;
using Domain.Interfaces.Repositories.Products;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Products;

public class ProductRepository(AppDbContext context) : GenericRepository<Product, Guid>(context), IProductRepository
{
    public async Task<(IReadOnlyList<Product> Products, int TotalCount)> SearchActiveProductsReadOnlyAsync(
        string? searchText, int pageIndex, int pageSize, string sortColumn, SortDirection sortDirection)
    {
        var query = Context.Products.AsNoTracking()
            .Where(p => p.IsActive);

        searchText = searchText?.Trim();
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(p =>
                p.Name.Contains(searchText) ||
                p.Brand!.Name.Contains(searchText) ||
                p.Categories!.Any(c => c.Name.Contains(searchText))
            );
        }

        var totalCount = await query.CountAsync();
        if (totalCount == 0)
        {
            return ([], 0);
        }

        return (await query
                .Include(p => p.ProductImages)
                .OrderBy(sortColumn, sortDirection)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
            totalCount);
    }

    public async Task<(IReadOnlyList<Product> Products, int TotalCount)> SearchActiveProductsBySlugReadOnlyAsync(
        string slug, string? searchText, int pageIndex, int pageSize, string sortColumn, SortDirection sortDirection)
    {
        var query = Context.Products.AsNoTracking()
            .Where(p =>
                p.IsActive && (
                    p.Brand!.Slug == slug ||
                    p.Categories!.Any(c => c.Slug == slug)
                )
            );

        searchText = searchText?.Trim();
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(p =>
                p.Name.Contains(searchText) ||
                p.Brand!.Name.Contains(searchText) ||
                p.Categories!.Any(c => c.Name.Contains(searchText))
            );
        }

        var totalCount = await query.CountAsync();
        if (totalCount == 0)
        {
            return ([], 0);
        }

        return (await query
                .Include(p => p.ProductImages)
                .OrderBy(sortColumn, sortDirection)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
            totalCount);
    }

    public async Task<Product?> GetBySlugWithDetailsReadOnlyAsync(string slug) =>
        await Context.Products.AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ProductImages)
            .Include(p => p.Brand)
            .Include(p => p.Categories!.Where(c => c.IsActive))
            .Include(p => p.ProductOptions!.Where(po => po.IsActive))
            .ThenInclude(po => po.ProductOptionValues!.Where(pov => pov.IsActive))
            .Include(p => p.ProductVariants)!
            .ThenInclude(pv => pv.ProductOptionValues)
            .FirstOrDefaultAsync(p => p.Slug == slug);
}