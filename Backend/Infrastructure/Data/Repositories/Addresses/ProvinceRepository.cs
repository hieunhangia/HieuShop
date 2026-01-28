using Domain.Entities.Addresses;
using Domain.Interfaces.Repositories.Addresses;

namespace Infrastructure.Data.Repositories.Addresses;

public class ProvinceRepository(AppDbContext context) : GenericRepository<Province, int>(context), IProvinceRepository;