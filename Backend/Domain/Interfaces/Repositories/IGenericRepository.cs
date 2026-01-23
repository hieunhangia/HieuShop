using Domain.Common;
using Domain.Enums;

namespace Domain.Interfaces.Repositories;

public interface IGenericRepository<in TKey, TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllReadOnlyAsync();

    Task<PagedAndSortedResultEntity<TEntity>> GetAllPagedAndSortedAsync(int pageIndex, int pageSize, string sortColumn,
        SortDirection sortDirection);

    Task<PagedAndSortedResultEntity<TEntity>> GetAllPagedAndSortedReadOnlyAsync(int pageIndex, int pageSize,
        string sortColumn, SortDirection sortDirection);

    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}