using Domain.Interfaces;
using Domain.Interfaces.Repositories.Products;
using Infrastructure.Data.Repositories.Products;

namespace Infrastructure.Data;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public IProductRepository Products => field ??= new ProductRepository(context);
    public IBrandRepository Brands => field ??= new BrandRepository(context);
    public ICategoryRepository Categories => field ??= new CategoryRepository(context);
    public Task<int> CompleteAsync() => context.SaveChangesAsync();
}