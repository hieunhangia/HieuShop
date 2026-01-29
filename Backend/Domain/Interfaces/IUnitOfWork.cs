using Domain.Interfaces.Repositories.Addresses;
using Domain.Interfaces.Repositories.Carts;
using Domain.Interfaces.Repositories.Orders;
using Domain.Interfaces.Repositories.Products;
using Domain.Interfaces.Repositories.Users;

namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IProvinceRepository Provinces { get; }
    IWardRepository Wards { get; }
    IUserShippingAddressRepository UserShippingAddresses { get; }
    IProductRepository Products { get; }
    IProductVariantRepository ProductVariants { get; }
    IBrandRepository Brands { get; }
    ICategoryRepository Categories { get; }
    ICartItemRepository CartItems { get; }
    IPaymentMethodRepository PaymentMethods { get; }

    Task<int> CompleteAsync();
}