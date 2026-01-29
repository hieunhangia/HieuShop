using Application.Features.PaymentMethods.DTOs;
using Domain.Entities.Orders;
using Riok.Mapperly.Abstractions;

namespace Application.Features.PaymentMethods;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PaymentMethodMapper
{
    public partial PaymentMethodDto MapToDto(PaymentMethod paymentMethod);
    public partial IReadOnlyList<PaymentMethodDto> MapToDtoList(IEnumerable<PaymentMethod> paymentMethods);
}