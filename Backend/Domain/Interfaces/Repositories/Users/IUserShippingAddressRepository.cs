using Domain.Entities.Users;

namespace Domain.Interfaces.Repositories.Users;

public interface IUserShippingAddressRepository : IGenericRepository<UserShippingAddress, Guid>
{
    Task<IReadOnlyList<UserShippingAddress>> GetListByUserIdWithDetailReadOnlyAsync(Guid userId);
    Task<UserShippingAddress?> GetByIdWithDetailReadOnlyAsync(Guid shippingAddressId);
}