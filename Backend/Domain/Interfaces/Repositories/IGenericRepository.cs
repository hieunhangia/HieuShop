namespace Domain.Interfaces.Repositories;

public interface IGenericRepository<TEntity, in TKey> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<List<TEntity>> GetAllAsync();
    Task<IReadOnlyList<TEntity>> GetAllReadOnlyAsync();
    Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
    Task<(IReadOnlyList<TEntity> Items, int TotalCount)> GetPagedReadOnlyAsync(int pageNumber, int pageSize);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TKey id);
}