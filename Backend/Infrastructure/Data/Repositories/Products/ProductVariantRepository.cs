using Domain.Entities.Products;
using Domain.Interfaces.Repositories.Products;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Products;

public class ProductVariantRepository(AppDbContext context)
    : GenericRepository<ProductVariant, Guid>(context), IProductVariantRepository
{
    public async Task<ProductVariant?> GetByIdWithProductInfoReadOnlyAsync(Guid id) =>
        await Context.ProductVariants.AsNoTracking()
            .Include(pv => pv.Product)
            .FirstOrDefaultAsync(pv => pv.Id == id);
}