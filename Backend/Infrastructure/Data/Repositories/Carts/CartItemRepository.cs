using Domain.Entities.Carts;
using Domain.Interfaces.Repositories.Carts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Carts;

public class CartItemRepository(AppDbContext context) : GenericRepository<CartItem, Guid>(context), ICartItemRepository
{
    public async Task<List<CartItem>> GetListWithDetailAsync(Guid userId) =>
        await Context.CartItems
            .Include(ci => ci.ProductVariant)
            .ThenInclude(pv => pv!.ProductOptionValues)!
            .ThenInclude(pov => pov.ProductOption)
            .Include(ci => ci.ProductVariant)
            .ThenInclude(pv => pv!.Product)
            .Where(x => x.UserId == userId)
            .ToListAsync();

    public async Task<CartItem?> GetByIdWithProductInfoAsync(Guid cartItemId) =>
        await Context.CartItems
            .Include(ci => ci.ProductVariant)
            .ThenInclude(pv => pv!.Product)
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

    public async Task<int> CountCartItemsAsync(Guid userId) =>
        await Context.CartItems.CountAsync(ci => ci.UserId == userId);

    public async Task<CartItem?> GetByUserIdAndProductVariantIdAsync(Guid userId, Guid productVariantId) =>
        await Context.CartItems.FirstOrDefaultAsync(ci =>
            ci.UserId == userId && ci.ProductVariantId == productVariantId);
}