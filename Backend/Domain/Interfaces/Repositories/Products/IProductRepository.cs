using Domain.Common;
using Domain.Entities.Products;
using Domain.Enums;

namespace Domain.Interfaces.Repositories.Products;

public interface IProductRepository : IGenericRepository<Guid, Product>
{
    Task<PagedResultEntity<Product>> GetAllActiveWithDefaultVariantPagedReadOnlyAsync(int pageIndex, int pageSize,
        string sortColumn, SortDirection sortDirection);

    Task<Product?> GetBySlugWithDetailsAsync(string slug);
}