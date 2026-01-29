using Domain.Interfaces;
using Domain.Interfaces.Repositories.Addresses;
using Domain.Interfaces.Repositories.Carts;
using Domain.Interfaces.Repositories.Orders;
using Domain.Interfaces.Repositories.Products;
using Domain.Interfaces.Repositories.Users;
using Infrastructure.Data.Repositories.Addresses;
using Infrastructure.Data.Repositories.Carts;
using Infrastructure.Data.Repositories.Orders;
using Infrastructure.Data.Repositories.Products;
using Infrastructure.Data.Repositories.Users;

namespace Infrastructure.Data;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public IProvinceRepository Provinces => field ??= new ProvinceRepository(context);
    public IWardRepository Wards => field ??= new WardRepository(context);
    public IUserShippingAddressRepository UserShippingAddresses => field ??= new UserShippingAddressRepository(context);
    public IProductRepository Products => field ??= new ProductRepository(context);
    public IProductVariantRepository ProductVariants => field ??= new ProductVariantRepository(context);
    public IBrandRepository Brands => field ??= new BrandRepository(context);
    public ICategoryRepository Categories => field ??= new CategoryRepository(context);
    public ICartItemRepository CartItems => field ??= new CartItemRepository(context);
    public IPaymentMethodRepository PaymentMethods => field ??= new PaymentMethodRepository(context);

    public Task<int> CompleteAsync() => context.SaveChangesAsync();
}