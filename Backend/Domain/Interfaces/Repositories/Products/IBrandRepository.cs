using Domain.Entities.Products;

namespace Domain.Interfaces.Repositories.Products;

public interface IBrandRepository : IGenericRepository<Brand, Guid>
{
    Task<IReadOnlyList<Brand>> GetTopActiveBrandsReadOnlyAsync(int top);
}