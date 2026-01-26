using Domain.Entities.Products;
using Domain.Enums;

namespace Domain.Interfaces.Repositories.Products;

public interface IProductRepository : IGenericRepository<Product, Guid>
{
    Task<(IReadOnlyList<Product> Products, int TotalCount)> SearchActiveProductsReadOnlyAsync(string? searchText,
        int pageIndex, int pageSize, string sortColumn, SortDirection sortDirection);

    Task<(IReadOnlyList<Product> Products, int TotalCount)> SearchActiveProductsByBrandSlugReadOnlyAsync(
        string brandSlug, string? searchText,
        int pageIndex, int pageSize, string sortColumn, SortDirection sortDirection);

    Task<(IReadOnlyList<Product> Products, int TotalCount)> SearchActiveProductsByCategorySlugReadOnlyAsync(
        string categorySlug, string? searchText,
        int pageIndex, int pageSize, string sortColumn, SortDirection sortDirection);

    Task<Product?> GetBySlugWithDetailsReadOnlyAsync(string slug);
}