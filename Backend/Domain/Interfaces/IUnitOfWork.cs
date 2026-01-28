using Domain.Interfaces.Repositories.Addresses;
using Domain.Interfaces.Repositories.Carts;
using Domain.Interfaces.Repositories.Products;

namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IProvinceRepository Provinces { get; }
    IWardRepository Wards { get; }
    IProductRepository Products { get; }
    IProductVariantRepository ProductVariants { get; }
    IBrandRepository Brands { get; }
    ICategoryRepository Categories { get; }
    ICartItemRepository CartItems { get; }

    Task<int> CompleteAsync();
}