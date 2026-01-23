using Domain.Common;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class GenericRepository<TKey, TEntity>(AppDbContext dbContext)
    : IGenericRepository<TKey, TEntity> where TEntity : class
{
    protected readonly AppDbContext dbContext = dbContext;

    public async Task<TEntity?> GetByIdAsync(TKey id) => await dbContext.Set<TEntity>().FindAsync(id);

    public async Task<IEnumerable<TEntity>> GetAllAsync() => await dbContext.Set<TEntity>().ToListAsync();

    public async Task<IEnumerable<TEntity>> GetAllReadOnlyAsync() =>
        await dbContext.Set<TEntity>().AsNoTracking().ToListAsync();

    public async Task<PagedResultEntity<TEntity>> GetAllPagedAndSortedAsync(int pageIndex, int pageSize,
        string sortColumn, SortDirection sortDirection) =>
        new(await dbContext.Set<TEntity>()
                .OrderBy(sortColumn, sortDirection)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
            await dbContext.Set<TEntity>().CountAsync(),
            pageIndex, pageSize);

    public async Task<PagedResultEntity<TEntity>> GetAllPagedAndSortedReadOnlyAsync(int pageIndex, int pageSize,
        string sortColumn, SortDirection sortDirection) =>
        new(await dbContext.Set<TEntity>()
                .AsNoTracking()
                .OrderBy(sortColumn, sortDirection)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
            await dbContext.Set<TEntity>().CountAsync(),
            pageIndex, pageSize);

    public void Add(TEntity entity) => dbContext.Set<TEntity>().Add(entity);

    public void Update(TEntity entity) => dbContext.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity) => dbContext.Set<TEntity>().Remove(entity);
}