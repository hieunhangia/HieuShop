using Application.Features.Identity;
using Application.Features.Identity.DTOs;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers;

[ApiController]
public class IdentityController(
    IIdentityService identityService,
    SignInManager<AppUser> signInManager,
    IOptionsMonitor<BearerTokenOptions> bearerTokenOptions
) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request) =>
        Created(string.Empty, new { id = await identityService.RegisterAsync(request) });

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, [FromQuery] bool? useCookies)
    {
        await identityService.LoginAsync(request, useCookies == true);
        return Empty;
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request, [FromQuery] bool? useCookies)
    {
        await identityService.GoogleLoginAsync(request, useCookies == true);
        return Empty;
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest tokenRequest)
    {
        var refreshTokenProtector = bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(tokenRequest.RefreshToken);

        if (refreshTicket?.Properties.ExpiresUtc is not { } expiresUtc ||
            DateTime.UtcNow >= expiresUtc ||
            await signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not { } user)
        {
            return Challenge();
        }

        var newPrincipal = await signInManager.CreateUserPrincipalAsync(user);
        return SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
    }

    [HttpPost("send-confirmation-email")]
    [Authorize]
    public async Task<IActionResult> SendConfirmationEmail()
    {
        await identityService.SendConfirmationEmailAsync();
        return Ok();
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        await identityService.ConfirmEmailAsync(request);
        return Ok();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        await identityService.ForgotPasswordAsync(request);
        return Ok();
    }

    [HttpPost("validate-reset-password-request")]
    public async Task<IActionResult> ValidateResetPasswordRequest([FromBody] ValidateResetPasswordRequest request)
    {
        await identityService.ValidateResetPasswordRequestAsync(request);
        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        await identityService.ResetPasswordAsync(request);
        return Ok();
    }

    [HttpPost("cookie-logout")]
    [Authorize]
    public async Task<IActionResult> CookieLogout()
    {
        await identityService.CookieLogoutAsync();
        return Ok();
    }

    [HttpGet("info")]
    [Authorize]
    public async Task<IActionResult> Info() => Ok(await identityService.GetUserInfoAsync());

    [HttpPost("set-password")]
    [Authorize]
    public async Task<IActionResult> SetPassword([FromBody] SetPasswordRequest request)
    {
        await identityService.SetPasswordAsync(request);
        return Ok();
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        await identityService.ChangePasswordAsync(request);
        return Ok();
    }
}