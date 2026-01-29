using Application.Features.PaymentMethods.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.PaymentMethods.Queries.GetActivePaymentMethods;

public class GetActivePaymentMethodsQueryHandler(IUnitOfWork unitOfWork, PaymentMethodMapper mapper)
    : IRequestHandler<GetActivePaymentMethodsQuery, IReadOnlyList<PaymentMethodDto>>
{
    public async Task<IReadOnlyList<PaymentMethodDto>> Handle(GetActivePaymentMethodsQuery request,
        CancellationToken cancellationToken) =>
        mapper.MapToDtoList(await unitOfWork.PaymentMethods.GetActivePaymentMethodsReadOnlyAsync());
}