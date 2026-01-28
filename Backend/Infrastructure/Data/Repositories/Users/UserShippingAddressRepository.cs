using Domain.Entities.Users;
using Domain.Interfaces.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Users;

public class UserShippingAddressRepository(AppDbContext context)
    : GenericRepository<UserShippingAddress, Guid>(context), IUserShippingAddressRepository
{
    public async Task<IReadOnlyList<UserShippingAddress>> GetListByUserIdWithDetailReadOnlyAsync(Guid userId) =>
        await Context.UserShippingAddresses.AsNoTracking()
            .Include(a => a.Ward)
            .ThenInclude(w => w!.Province)
            .Where(a => a.UserId == userId)
            .ToListAsync();

    public async Task<UserShippingAddress?> GetByIdWithDetailReadOnlyAsync(Guid shippingAddressId) =>
        await Context.UserShippingAddresses.AsNoTracking()
            .Include(a => a.Ward)
            .ThenInclude(w => w!.Province)
            .FirstOrDefaultAsync(a => a.Id == shippingAddressId);
}