using Domain.Entities.Products;
using Domain.Enums;

namespace Domain.Interfaces.Repositories.Products;

public interface IProductRepository : IGenericRepository<Guid, Product>
{
    Task<(List<Product> Products, int TotalCount)> QueryActiveProductsReadOnlyAsync(string searchText, int pageIndex,
        int pageSize, string sortColumn, SortDirection sortDirection);

    Task<Product?> GetBySlugWithDetailsAsync(string slug);
}