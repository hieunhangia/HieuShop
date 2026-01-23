using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class GenericRepository<TEntity, TKey>(AppDbContext dbContext)
    : IGenericRepository<TEntity, TKey> where TEntity : class
{
    protected readonly AppDbContext dbContext = dbContext;

    public async Task<TEntity?> GetByIdAsync(TKey id) => await dbContext.Set<TEntity>().FindAsync(id);

    public async Task<IEnumerable<TEntity>> GetAllAsync() => await dbContext.Set<TEntity>().ToListAsync();

    public async Task<IEnumerable<TEntity>> GetAllReadOnlyAsync() =>
        await dbContext.Set<TEntity>().AsNoTracking().ToListAsync();

    public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = dbContext.Set<TEntity>();
        return (await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(), await query.CountAsync());
    }

    public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedReadOnlyAsync(int pageNumber, int pageSize)
    {
        var query = dbContext.Set<TEntity>().AsNoTracking();
        return (await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(), await query.CountAsync());
    }

    public void Add(TEntity entity) => dbContext.Set<TEntity>().Add(entity);

    public void Update(TEntity entity) => dbContext.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity) => dbContext.Set<TEntity>().Remove(entity);
}