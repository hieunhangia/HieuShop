using Domain.Entities.Users;
using Domain.Interfaces.Repositories.Users;

namespace Infrastructure.Data.Repositories.Users;

public class UserRepository(AppDbContext context) : GenericRepository<AppUser, Guid>(context), IUserRepository;