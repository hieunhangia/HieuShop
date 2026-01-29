using Domain.Entities.Orders;

namespace Domain.Interfaces.Repositories.Orders;

public interface IPaymentMethodRepository : IGenericRepository<PaymentMethod, Guid>
{
    Task<IReadOnlyList<PaymentMethod>> GetActivePaymentMethodsReadOnlyAsync();
}