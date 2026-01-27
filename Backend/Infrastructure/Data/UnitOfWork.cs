using Domain.Interfaces;
using Domain.Interfaces.Repositories.Carts;
using Domain.Interfaces.Repositories.Products;
using Infrastructure.Data.Repositories.Carts;
using Infrastructure.Data.Repositories.Products;

namespace Infrastructure.Data;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public IProductRepository Products => field ??= new ProductRepository(context);
    public IBrandRepository Brands => field ??= new BrandRepository(context);
    public ICategoryRepository Categories => field ??= new CategoryRepository(context);
    public ICartItemRepository CartItems => field ??= new CartItemRepository(context);

    public Task<int> CompleteAsync() => context.SaveChangesAsync();
}