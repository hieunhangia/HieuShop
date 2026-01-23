using Domain.Interfaces;
using Domain.Interfaces.Repositories.Products;
using Infrastructure.Data.Repositories.Products;

namespace Infrastructure.Data;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public IProductRepository Products => field ??= new ProductRepository(dbContext);

    public Task<int> CompleteAsync() => dbContext.SaveChangesAsync();
}