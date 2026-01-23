using Domain.Interfaces.Repositories.Products;

namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IBrandRepository Brands { get; }
    ICategoryRepository Categories { get; }

    Task<int> CompleteAsync();
}