using API.Extensions;
using Application.Features.Carts.Commands.AddProductVariantToCart;
using Application.Features.Carts.Commands.RemoveCartItem;
using Application.Features.Carts.Commands.UpdateCartItemQuantity;
using Application.Features.Carts.Queries.CountCartItems;
using Application.Features.Carts.Queries.GetCart;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("carts")]
[Authorize(Roles = UserRole.Customer)]
public class CartController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCart() => Ok(await sender.Send(new GetCartQuery { UserId = User.GetUserId() }));

    [HttpGet("count")]
    public async Task<IActionResult> CountCartItems() =>
        Ok(await sender.Send(new CountCartItemsQuery { UserId = User.GetUserId() }));

    [HttpPost]
    public async Task<IActionResult> AddProductVariantToCart([FromBody] AddProductVariantToCartRequest request)
    {
        await sender.Send(new AddProductVariantToCartCommand
        {
            UserId = User.GetUserId(),
            ProductVariantId = request.ProductVariantId
        });
        return Ok();
    }

    [HttpPut("{cartItemId:guid}/quantity")]
    public async Task<IActionResult> UpdateCartItemQuantity([FromRoute] Guid cartItemId,
        [FromBody] UpdateCartItemQuantityRequest request)
    {
        await sender.Send(new UpdateCartItemQuantityCommand
        {
            UserId = User.GetUserId(),
            CartItemId = cartItemId,
            Quantity = request.Quantity
        });
        return Ok();
    }

    [HttpDelete("{cartItemId:guid}")]
    public async Task<IActionResult> RemoveCartItem([FromRoute] Guid cartItemId)
    {
        await sender.Send(new RemoveCartItemCommand
        {
            UserId = User.GetUserId(),
            CartItemId = cartItemId
        });
        return NoContent();
    }
}