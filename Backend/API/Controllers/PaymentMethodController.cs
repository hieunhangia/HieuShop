using Application.Features.PaymentMethods.Queries.GetActivePaymentMethods;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("payment-methods")]
public class PaymentMethodController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = UserRole.Customer)]
    public async Task<IActionResult> GetActivePaymentMethods() =>
        Ok(await sender.Send(new GetActivePaymentMethodsQuery()));
}