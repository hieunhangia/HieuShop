using System.Security.Claims;
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
    public async Task<IActionResult> GetCart()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        return Ok(await sender.Send(new GetCartQuery { UserId = Guid.Parse(userId) }));
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> CountCartItems()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        return Ok(await sender.Send(new CountCartItemsQuery { UserId = Guid.Parse(userId) }));
    }

    [HttpPost]
    public async Task<IActionResult> AddProductVariantToCart([FromBody] Guid productVariantId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        await sender.Send(new AddProductVariantToCartCommand
        {
            UserId = Guid.Parse(userId),
            ProductVariantId = productVariantId
        });
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveCartItem([FromBody] Guid productVariantId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        await sender.Send(new RemoveCartItemCommand
        {
            UserId = Guid.Parse(userId),
            ProductVariantId = productVariantId
        });
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCartItemQuantity([FromBody] UpdateCartItemQuantityCommand command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        await sender.Send(new UpdateCartItemQuantityCommand
        {
            UserId = Guid.Parse(userId),
            ProductVariantId = command.ProductVariantId,
            NewQuantity = command.NewQuantity
        });
        return Ok();
    }
}