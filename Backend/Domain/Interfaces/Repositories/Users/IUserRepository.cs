using Domain.Entities.Users;

namespace Domain.Interfaces.Repositories.Users;

public interface IUserRepository : IGenericRepository<AppUser, Guid>;