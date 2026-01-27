using Application.Common.Exceptions;
using Domain.Entities.Carts;
using Domain.Interfaces.Repositories.Carts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Carts;

public class CartItemRepository(AppDbContext context) : GenericRepository<CartItem, Guid>(context), ICartItemRepository
{
    public async Task<int> CountCartItemsAsync(Guid userId) =>
        await Context.CartItems.CountAsync(ci => ci.UserId == userId);

    public async Task<(IReadOnlyList<CartItem> CartItems, string? WarningMessage)> GetCartAsync(Guid userId)
    {
        var cartItems = await Context.CartItems
            .Include(ci => ci.ProductVariant)
            .ThenInclude(pv => pv!.ProductOptionValues)!
            .ThenInclude(pov => pov.ProductOption)
            .Include(ci => ci.ProductVariant)
            .ThenInclude(pv => pv!.Product)
            .Where(x => x.UserId == userId)
            .ToListAsync();

        var isCartUpdated = false;
        var itemsToRemove = new List<CartItem>();
        foreach (var cartItem in cartItems)
        {
            if (!cartItem.ProductVariant!.Product!.IsActive || !cartItem.ProductVariant!.IsActive ||
                cartItem.ProductVariant.AvailableStock <= 0)
            {
                itemsToRemove.Add(cartItem);
                continue;
            }

            if (cartItem.Quantity <= cartItem.ProductVariant.AvailableStock) continue;
            cartItem.Quantity = cartItem.ProductVariant.AvailableStock;
            isCartUpdated = true;
        }

        if (itemsToRemove.Count != 0)
        {
            Context.CartItems.RemoveRange(itemsToRemove);
            cartItems.RemoveAll(x => itemsToRemove.Contains(x));
            isCartUpdated = true;
        }

        if (!isCartUpdated) return (cartItems, null);

        await Context.SaveChangesAsync();
        return (cartItems,
            "Giỏ hàng của bạn đã được cập nhật do một số sản phẩm đã không còn khả dụng hoặc số lượng vượt quá tồn kho.");
    }

    public async Task AddProductVariantToCartAsync(Guid userId, Guid productVariantId)
    {
        if (await Context.ProductVariants.AsNoTracking()
                .Include(pv => pv.Product)
                .FirstOrDefaultAsync(pv => pv.Id == productVariantId) is { } productVariant)
        {
            if (!productVariant.Product!.IsActive || !productVariant.IsActive)
            {
                throw new BadRequestException("Không thể thêm sản phẩm vào giỏ hàng vì sản phẩm không khả dụng.");
            }

            if (await Context.CartItems.FirstOrDefaultAsync(ci =>
                    ci.UserId == userId && ci.ProductVariantId == productVariantId) is { } cartItem)
            {
                if (cartItem.Quantity >= productVariant.AvailableStock)
                {
                    throw new BadRequestException(
                        "Không thể thêm sản phẩm vào giỏ hàng vì đã đạt đến số lượng tối đa.");
                }

                cartItem.Quantity++;
            }
            else
            {
                if (productVariant.AvailableStock <= 0)
                {
                    throw new BadRequestException(
                        "Không thể thêm sản phẩm vào giỏ hàng vì đã đạt đến số lượng tối đa.");
                }

                await Context.CartItems.AddAsync(new CartItem
                {
                    UserId = userId,
                    ProductVariantId = productVariantId,
                    Quantity = 1
                });
            }
        }
        else
        {
            throw new NotFoundException("Không tìm thấy biến thể sản phẩm.");
        }
    }

    public async Task RemoveCartItemAsync(Guid userId, Guid productVariantId)
    {
        if (await Context.CartItems.FirstOrDefaultAsync(ci =>
                ci.UserId == userId && ci.ProductVariantId == productVariantId) is { } cartItem)
        {
            Context.CartItems.Remove(cartItem);
        }
    }

    public async Task UpdateCartItemQuantityAsync(Guid userId, Guid productVariantId, int quantity)
    {
        if (await Context.CartItems
                .Include(ci => ci.ProductVariant)
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductVariantId == productVariantId) is
            { } cartItem)
        {
            if (quantity > cartItem.ProductVariant!.AvailableStock)
            {
                throw new BadRequestException("Không thể cập nhật số lượng sản phẩm vì vượt quá tồn kho.");
            }

            cartItem.Quantity = quantity;
        }
        else
        {
            throw new NotFoundException("Không tìm thấy sản phẩm trong giỏ hàng.");
        }
    }
}