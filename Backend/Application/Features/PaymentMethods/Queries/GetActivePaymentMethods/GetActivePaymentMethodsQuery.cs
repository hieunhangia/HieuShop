using Application.Features.PaymentMethods.DTOs;
using MediatR;

namespace Application.Features.PaymentMethods.Queries.GetActivePaymentMethods;

public class GetActivePaymentMethodsQuery : IRequest<IReadOnlyList<PaymentMethodDto>>;