using Domain.Entities.Products;

namespace Domain.Interfaces.Repositories.Products;

public interface ICategoryRepository : IGenericRepository<Category, Guid>
{
    Task<IReadOnlyList<Category>> GetTopActiveCategoriesReadOnlyAsync(int top);
}