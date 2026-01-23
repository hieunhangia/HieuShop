namespace Domain.Interfaces.Repositories;

public interface IGenericRepository<TEntity, in TKey> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllReadOnlyAsync();
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}