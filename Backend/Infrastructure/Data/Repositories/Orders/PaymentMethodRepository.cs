using Domain.Entities.Orders;
using Domain.Interfaces.Repositories.Orders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Orders;

public class PaymentMethodRepository(AppDbContext context)
    : GenericRepository<PaymentMethod, Guid>(context), IPaymentMethodRepository
{
    public async Task<IReadOnlyList<PaymentMethod>> GetActivePaymentMethodsReadOnlyAsync() =>
        await Context.PaymentMethods.AsNoTracking()
            .Where(pm => pm.IsActive)
            .ToListAsync();
}