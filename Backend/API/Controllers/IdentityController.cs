using System.Text;
using System.Text.Encodings.Web;
using Application.DTOs.Identity;
using Application.Interfaces;
using Domain.Constants;
using Domain.Entities.Users;
using Google.Apis.Auth;
using Infrastructure.Data;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace API.Controllers;

[ApiController]
public class IdentityController(
    AppDbContext dbContext,
    IConfiguration configuration,
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IEmailService emailService,
    IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
    TimeProvider timeProvider
) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registration)
    {
        var user = new AppUser
        {
            UserName = registration.Email,
            Email = registration.Email
        };

        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        var result = await userManager.CreateAsync(user, registration.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.ToList();

            if (errors.Any(e => e.Code == "InvalidEmail") &&
                errors.Any(e => e.Code == "InvalidUserName"))
            {
                errors.RemoveAll(e => e.Code == "InvalidUserName");
            }

            if (errors.Any(e => e.Code == "DuplicateEmail") &&
                errors.Any(e => e.Code == "DuplicateUserName"))
            {
                errors.RemoveAll(e => e.Code == "DuplicateUserName");
            }

            return CreateValidationProblem(IdentityResult.Failed(errors.ToArray()));
        }

        result = await userManager.AddToRoleAsync(user, UserRole.Customer);
        if (!result.Succeeded)
        {
            return CreateValidationProblem(result);
        }

        await transaction.CommitAsync();
        return Created(string.Empty, new { user.Id, user.Email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login, [FromQuery] bool? useCookies)
    {
        signInManager.AuthenticationScheme =
            useCookies == true ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, useCookies == true,
            lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            return Problem(title: "Lỗi Đăng Nhập",
                detail: "Tài khoản của bạn đã bị khóa do nhiều lần đăng nhập thất bại. Vui lòng thử lại sau.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        if (!result.Succeeded)
        {
            return Problem(title: "Lỗi Đăng Nhập",
                detail: "Thông tin đăng nhập không hợp lệ. Vui lòng kiểm tra và thử lại.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        return Empty;
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request, [FromQuery] bool? useCookies)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken,
                new GoogleJsonWebSignature.ValidationSettings { Audience = [configuration["GoogleClientId"]] });

            if (!payload.EmailVerified)
            {
                return Problem(title: "Đăng Nhập Bằng Google Thất Bại",
                    detail: "Email của tài khoản Google chưa được xác minh.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var user = await userManager.FindByLoginAsync(LoginProvider.Google, payload.Subject);
            if (user == null)
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync();

                user = await userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new AppUser
                    {
                        UserName = payload.Email,
                        Email = payload.Email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        return CreateValidationProblem(result);
                    }

                    result = await userManager.AddToRoleAsync(user, UserRole.Customer);
                    if (!result.Succeeded)
                    {
                        return CreateValidationProblem(result);
                    }

                    result = await userManager.AddLoginAsync(user,
                        new UserLoginInfo(LoginProvider.Google, payload.Subject,
                            LoginProvider.Google));
                    if (!result.Succeeded)
                    {
                        return CreateValidationProblem(result);
                    }
                }
                else
                {
                    var result = await userManager.AddLoginAsync(user,
                        new UserLoginInfo(LoginProvider.Google, payload.Subject,
                            LoginProvider.Google));
                    if (!result.Succeeded)
                    {
                        return CreateValidationProblem(result);
                    }

                    if (!user.EmailConfirmed)
                    {
                        user.EmailConfirmed = true;
                        result = await userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            return CreateValidationProblem(result);
                        }
                    }
                }

                await transaction.CommitAsync();
            }

            signInManager.AuthenticationScheme = useCookies == true
                ? IdentityConstants.ApplicationScheme
                : IdentityConstants.BearerScheme;
            await signInManager.SignInAsync(user, useCookies == true);
            return Empty;
        }
        catch (InvalidJwtException)
        {
            return Problem(title: "Đăng Nhập Bằng Google Thất Bại",
                detail: "Đăng nhập bằng Google không thành công. Vui lòng thử lại.",
                statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest refreshRequest)
    {
        var refreshTokenProtector = bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);

        if (refreshTicket?.Properties.ExpiresUtc is not { } expiresUtc ||
            timeProvider.GetUtcNow() >= expiresUtc ||
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
        if (await userManager.GetUserAsync(User) is not { } user)
        {
            return Unauthorized();
        }

        if (user.EmailConfirmed)
        {
            return Problem(title: "Email đã được xác nhận",
                detail: "Email của bạn đã được xác nhận trước đó.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var clientUrl = configuration["ClientUrl"] ??
                        throw new InvalidOperationException("ClientUrl is not configured.");
        var confirmEmailPath = configuration["ConfirmEmailPath"] ??
                               throw new InvalidOperationException("ConfirmEmailPath is not configured.");

        var code = WebEncoders.Base64UrlEncode(
            Encoding.UTF8.GetBytes(await userManager.GenerateEmailConfirmationTokenAsync(user)));

        var confirmEmailUrl =
            $"{clientUrl.TrimEnd('/')}/{confirmEmailPath.TrimStart('/')}?email={user.Email}&code={code}";

        await SendEmailConfirmationLinkAsync(user.Email!, HtmlEncoder.Default.Encode(confirmEmailUrl));

        return Ok();
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest confirmEmailRequest)
    {
        if (await userManager.FindByEmailAsync(confirmEmailRequest.Email) is not { } user)
        {
            return Problem(title: "Xác Nhận Email Thất Bại",
                detail: "Xác nhận email không thành công. Vui lòng thử lại.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        if (user.EmailConfirmed)
        {
            return Ok();
        }

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmEmailRequest.Code));
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return Problem(title: "Xác Nhận Email Thất Bại",
                    detail: "Xác nhận email không thành công. Vui lòng thử lại.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            return Ok();
        }
        catch (FormatException)
        {
            return Problem(title: "Xác Nhận Email Thất Bại",
                detail: "Xác nhận email không thành công. Vui lòng thử lại.",
                statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest resetRequest)
    {
        if (await userManager.FindByEmailAsync(resetRequest.Email) is not { } user)
        {
            return Ok();
        }

        var clientUrl = configuration["ClientUrl"] ??
                        throw new InvalidOperationException("ClientUrl is not configured.");
        var resetPasswordPath = configuration["ResetPasswordPath"] ??
                                throw new InvalidOperationException("ResetPasswordPath is not configured.");

        var code = WebEncoders.Base64UrlEncode(
            Encoding.UTF8.GetBytes(await userManager.GeneratePasswordResetTokenAsync(user)));

        var resetPasswordUrl =
            $"{clientUrl.TrimEnd('/')}/{resetPasswordPath.TrimStart('/')}?email={user.Email}&code={code}";

        await SendPasswordResetLinkAsync(user.Email!, HtmlEncoder.Default.Encode(resetPasswordUrl));

        return Ok();
    }

    [HttpPost("validate-reset-password-request")]
    public async Task<IActionResult> ValidateResetPasswordRequest([FromBody] ValidateResetPasswordRequest request)
    {
        if (await userManager.FindByEmailAsync(request.Email) is not { } user)
        {
            return Problem(title: "Yêu Cầu Đặt Lại Mật Khẩu Không Hợp Lệ",
                detail: "Yêu cầu đặt lại mật khẩu không hợp lệ. Vui lòng thử lại.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetCode));
            var result = await userManager.VerifyUserTokenAsync(user,
                userManager.Options.Tokens.PasswordResetTokenProvider, UserManager<AppUser>.ResetPasswordTokenPurpose,
                code);
            if (!result)
            {
                return Problem(title: "Yêu Cầu Đặt Lại Mật Khẩu Không Hợp Lệ",
                    detail: "Yêu cầu đặt lại mật khẩu không hợp lệ. Vui lòng thử lại.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            return Ok();
        }
        catch (FormatException)
        {
            return Problem(title: "Yêu Cầu Đặt Lại Mật Khẩu Không Hợp Lệ",
                detail: "Yêu cầu đặt lại mật khẩu không hợp lệ. Vui lòng thử lại.",
                statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetRequest)
    {
        if (await userManager.FindByEmailAsync(resetRequest.Email) is not { } user)
        {
            return Problem(title: "Đặt Lại Mật Khẩu Thất Bại",
                detail: "Đặt lại mật khẩu không thành công. Vui lòng thử lại.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetRequest.ResetCode));
            var result = await userManager.ResetPasswordAsync(user, code, resetRequest.NewPassword);
            return result.Succeeded ? Ok() : CreateValidationProblem(result);
        }
        catch (FormatException)
        {
            return Problem(title: "Đặt Lại Mật Khẩu Thất Bại",
                detail: "Đặt lại mật khẩu không thành công. Vui lòng thử lại.",
                statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost("cookie-logout")]
    [Authorize]
    public async Task<IActionResult> CookieLogout()
    {
        await signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet("info")]
    [Authorize]
    public async Task<IActionResult> Info()
    {
        if (await userManager.GetUserAsync(User) is not { } user)
        {
            return Unauthorized();
        }

        return Ok(new UserInfoResponse
        {
            Email = user.Email ?? string.Empty,
            EmailConfirmed = user.EmailConfirmed,
            HasPassword = !string.IsNullOrEmpty(user.PasswordHash),
            Roles = (await userManager.GetRolesAsync(user)).ToList()
        });
    }

    [HttpPost("set-password")]
    [Authorize]
    public async Task<IActionResult> SetPassword([FromBody] SetPasswordRequest setPasswordRequest)
    {
        if (await userManager.GetUserAsync(User) is not { } user)
        {
            return Unauthorized();
        }

        if (!string.IsNullOrEmpty(user.PasswordHash))
        {
            return Problem(title: "Thiết Lập Mật Khẩu Thất Bại",
                detail: "Mật khẩu đã được thiết lập cho tài khoản này.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var result = await userManager.AddPasswordAsync(user, setPasswordRequest.NewPassword);
        return result.Succeeded ? Ok() : CreateValidationProblem(result);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
    {
        if (await userManager.GetUserAsync(User) is not { } user)
        {
            return Unauthorized();
        }

        if (string.IsNullOrEmpty(user.PasswordHash))
        {
            return Problem(title: "Đổi Mật Khẩu Thất Bại",
                detail: "Tài khoản này chưa dùng mật khẩu. Vui lòng thiết lập mật khẩu trước.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var result = await userManager.ChangePasswordAsync(user, changePasswordRequest.OldPassword,
            changePasswordRequest.NewPassword);
        return result.Succeeded ? Ok() : CreateValidationProblem(result);
    }

    private Task SendEmailConfirmationLinkAsync(string email, string confirmationLink) =>
        emailService.SendEmailAsync(email, "Confirm your email",
            $"""
             <html lang="en">
             <head>
             </head>
             <body>
                Vui lòng xác nhận tài khoản của bạn bằng cách <a href='{confirmationLink}'>nhấp vào đây</a>.<br>
                Liên kết này có hiệu lực trong {TokenExpiredTime.EmailConfirmationHours} giờ.<br>
                Nếu bạn không yêu cầu việc này, bạn có thể bỏ qua email này.
             </body>
             </html>
             """);

    private Task SendPasswordResetLinkAsync(string email, string resetLink) =>
        emailService.SendEmailAsync(email, "Reset your password",
            $"""
             <html lang="en">
             <head>
             </head>
             <body>
                Vui lòng đặt lại mật khẩu của bạn bằng cách <a href='{resetLink}'>nhấp vào đây</a>.<br>
                Liên kết này có hiệu lực trong {TokenExpiredTime.PasswordResetCodeMinutes} phút.<br>
                Nếu bạn không yêu cầu việc này, bạn có thể bỏ qua email này.
             </body>
             </html>
             """);

    private ActionResult CreateValidationProblem(IdentityResult result)
    {
        var modelState = new ModelStateDictionary();
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(modelState);
    }
}