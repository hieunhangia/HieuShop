using Domain.Entities.Addresses;

namespace Domain.Interfaces.Repositories.Addresses;

public interface IWardRepository : IGenericRepository<Ward, int>
{
    Task<IReadOnlyList<Ward>> GetListByProvinceIdReadOnlyAsync(int provinceId);
}