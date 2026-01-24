using Domain.Entities.Products;

namespace Domain.Interfaces.Repositories.Products;

public interface ICategoryRepository : IGenericRepository<Category, Guid>
{
    Task<IEnumerable<Category>> QueryActiveCategoriesReadOnlyAsync(int top);
}