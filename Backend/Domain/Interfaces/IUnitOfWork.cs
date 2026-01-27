using Domain.Interfaces.Repositories.Carts;
using Domain.Interfaces.Repositories.Products;

namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IProductVariantRepository ProductVariants { get; }
    IBrandRepository Brands { get; }
    ICategoryRepository Categories { get; }
    ICartItemRepository CartItems { get; }

    Task<int> CompleteAsync();
}