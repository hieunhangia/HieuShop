using Domain.Entities.Carts;

namespace Domain.Interfaces.Repositories.Carts;

public interface ICartItemRepository : IGenericRepository<CartItem, Guid>
{
    Task<List<CartItem>> GetListWithDetailAsync(Guid userId);
    Task<CartItem?> GetByIdWithProductInfoAsync(Guid cartItemId);
    Task<int> CountCartItemsAsync(Guid userId);
    Task<CartItem?> GetByUserIdAndProductVariantIdAsync(Guid userId, Guid productVariantId);
}