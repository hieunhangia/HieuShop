using Domain.Entities.Carts;

namespace Domain.Interfaces.Repositories.Carts;

public interface ICartItemRepository : IGenericRepository<CartItem, Guid>
{
    Task<int> CountCartItemsAsync(Guid userId);
    Task<(IReadOnlyList<CartItem> CartItems, string? WarningMessage)> GetCartAsync(Guid userId);
    Task AddProductVariantToCartAsync(Guid userId, Guid productVariantId);
    Task RemoveCartItemAsync(Guid userId, Guid productVariantId);
    Task UpdateCartItemQuantityAsync(Guid userId, Guid productVariantId, int quantity);
}