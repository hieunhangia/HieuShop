using Domain.Entities.Products;

namespace Domain.Interfaces.Repositories.Products;

public interface IProductVariantRepository : IGenericRepository<ProductVariant, Guid>
{
    Task<ProductVariant?> GetByIdWithProductInfoReadOnlyAsync(Guid id);
}