using Domain.Entities.Carts;

namespace Domain.Interfaces.Repositories.Carts;

public interface ICartItemRepository : IGenericRepository<CartItem, Guid>
{
    Task<(IReadOnlyList<CartItem> CartItems, string? WarningMessage)> GetCartAsync(Guid userId);
    Task<int> CountCartItemsAsync(Guid userId);
    Task<bool> IsCartItemBelongToUserAsync(Guid userId, Guid cartItemId);
    Task AddProductVariantToCartAsync(Guid userId, Guid productVariantId);
    Task RemoveCartItemAsync(Guid cartItemId);
    Task UpdateCartItemQuantityAsync(Guid cartItemId, int quantity);
}