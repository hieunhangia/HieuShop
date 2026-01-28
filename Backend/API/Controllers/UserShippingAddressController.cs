using API.Extensions;
using Application.Features.UserShippingAddresses.Commands.AddUserShippingAddress;
using Application.Features.UserShippingAddresses.Commands.RemoveUserShippingAddress;
using Application.Features.UserShippingAddresses.Commands.UpdateUserShippingAddress;
using Application.Features.UserShippingAddresses.Queries.GetUserShippingAddress;
using Application.Features.UserShippingAddresses.Queries.GetUserShippingAddresses;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("users/shipping-addresses")]
[Authorize(Roles = UserRole.Customer)]
public class UserShippingAddressController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUserShippingAddresses() =>
        Ok(await sender.Send(new GetUserShippingAddressesQuery { UserId = User.GetUserId() }));

    [HttpGet("{shippingAddressId:guid}")]
    public async Task<IActionResult> GetUserShippingAddressById([FromRoute] Guid shippingAddressId) =>
        Ok(await sender.Send(new GetUserShippingAddressQuery
            { UserId = User.GetUserId(), ShippingAddressId = shippingAddressId }));

    [HttpPost]
    public async Task<IActionResult> AddUserShippingAddress([FromBody] AddUserShippingAddressRequest request)
    {
        await sender.Send(new AddUserShippingAddressCommand
        {
            UserId = User.GetUserId(),
            RecipientName = request.RecipientName,
            RecipientPhone = request.RecipientPhone,
            DetailAddress = request.DetailAddress,
            WardId = request.WardId
        });
        return Ok();
    }

    [HttpPut("{shippingAddressId:guid}")]
    public async Task<IActionResult> UpdateUserShippingAddress([FromRoute] Guid shippingAddressId,
        [FromBody] AddUserShippingAddressRequest request)
    {
        await sender.Send(new UpdateUserShippingAddressCommand
        {
            UserId = User.GetUserId(),
            ShippingAddressId = shippingAddressId,
            RecipientName = request.RecipientName,
            RecipientPhone = request.RecipientPhone,
            DetailAddress = request.DetailAddress,
            WardId = request.WardId
        });
        return Ok();
    }

    [HttpDelete("{shippingAddressId:guid}")]
    public async Task<IActionResult> RemoveUserShippingAddress([FromRoute] Guid shippingAddressId)
    {
        await sender.Send(new RemoveUserShippingAddressCommand
            { UserId = User.GetUserId(), ShippingAddressId = shippingAddressId });
        return NoContent();
    }
}