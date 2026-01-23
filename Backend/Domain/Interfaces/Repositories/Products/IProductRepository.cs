using Domain.Entities.Products;
using Domain.Enums;

namespace Domain.Interfaces.Repositories.Products;

public interface IProductRepository : IGenericRepository<Product, Guid>
{
    Task<(IEnumerable<Product> Products, int TotalCount)> QueryActiveProductsReadOnlyAsync(string searchText,
        int pageIndex, int pageSize, string sortColumn, SortDirection sortDirection);

    Task<Product?> GetBySlugWithDetailsReadOnlyAsync(string slug);
}