using Domain.Interfaces.Repositories.Products;

namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IProductRepository Products { get; }

    Task<int> CompleteAsync();
}