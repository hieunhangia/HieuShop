using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class GenericRepository<TEntity, TKey>(AppDbContext context)
    : IGenericRepository<TEntity, TKey> where TEntity : class
{
    protected readonly AppDbContext Context = context;

    public async Task<TEntity?> GetByIdAsync(TKey id) => await Context.Set<TEntity>().FindAsync(id);

    public async Task<IEnumerable<TEntity>> GetAllAsync() => await Context.Set<TEntity>().ToListAsync();

    public async Task<IEnumerable<TEntity>> GetAllReadOnlyAsync() =>
        await Context.Set<TEntity>().AsNoTracking().ToListAsync();

    public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = Context.Set<TEntity>();
        return (await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(), await query.CountAsync());
    }

    public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedReadOnlyAsync(int pageNumber, int pageSize)
    {
        var query = Context.Set<TEntity>().AsNoTracking();
        return (await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(), await query.CountAsync());
    }

    public void Add(TEntity entity) => Context.Set<TEntity>().Add(entity);

    public void Update(TEntity entity) => Context.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity) => Context.Set<TEntity>().Remove(entity);
}