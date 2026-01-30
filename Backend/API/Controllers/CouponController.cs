using API.Extensions;
using Application.Features.Coupons.Commands.PurchaseCoupon;
using Application.Features.Coupons.Queries.GetLoyaltyPoints;
using Application.Features.Coupons.Queries.GetUserCoupons;
using Application.Features.Coupons.Queries.SearchActiveCoupons;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("coupons")]
[Authorize(Roles = UserRole.Customer)]
public class CouponController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetActiveCoupons([FromQuery] SearchActiveCouponsQuery query) =>
        Ok(await sender.Send(query));

    [HttpGet("/users/coupons")]
    public async Task<IActionResult> GetUserCoupons() =>
        Ok(await sender.Send(new GetUserCouponsQuery { UserId = User.GetUserId() }));

    [HttpGet("/loyalty-points")]
    public async Task<IActionResult> GetLoyaltyPoints() =>
        Ok(await sender.Send(new GetLoyaltyPointQuery { UserId = User.GetUserId() }));

    [HttpPost("{couponId:guid}/purchase")]
    public async Task<IActionResult> PurchaseCoupon([FromRoute] Guid couponId)
    {
        await sender.Send(new PurchaseCouponCommand { UserId = User.GetUserId(), CouponId = couponId });
        return Ok();
    }
}