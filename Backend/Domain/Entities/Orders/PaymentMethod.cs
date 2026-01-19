using Domain.Commons;

namespace Domain.Entities.Orders;

public class PaymentMethod : BaseAuditableEntity<Guid>
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}