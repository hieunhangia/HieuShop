using Domain.Common;

namespace Domain.Entities.Addresses;

public class Ward : BaseEntity<int>
{
    public required string Name { get; set; }
    public int ProvinceId { get; set; }

    public Province? Province { get; set; }
}