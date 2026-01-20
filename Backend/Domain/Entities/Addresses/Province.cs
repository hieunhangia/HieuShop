using Domain.Common;

namespace Domain.Entities.Addresses;

public class Province : BaseEntity<int>
{
    public required string Name { get; set; }

    public ICollection<Ward>? Wards { get; set; }
}