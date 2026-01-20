using System.Text;
using System.Text.Encodings.Web;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Features.Identity.Constants;
using Application.Features.Identity.DTOs;
using Domain.Constants;
using Domain.Entities.Users;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using UnauthorizedAccessException = Application.Common.Exceptions.UnauthorizedAccessException;

namespace Application.Features.Identity;

public class IdentityService(
    IConfiguration configuration,
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IEmailService emailService
) : IIdentityService
{
    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);
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

            throw new ValidationException(GetErrors(IdentityResult.Failed(errors.ToArray())));
        }

        result = await userManager.AddToRoleAsync(user, UserRole.Customer);
        return !result.Succeeded
            ? throw new ValidationException(GetErrors(result))
            : user.Id.ToString();
    }

    public async Task LoginAsync(LoginRequest request, bool useCookies)
    {
        signInManager.AuthenticationScheme =
            useCookies ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result =
            await signInManager.PasswordSignInAsync(request.Email, request.Password, useCookies,
                lockoutOnFailure: true);
        if (result.IsLockedOut)
        {
            throw new BadRequestException("Tài khoản bị khóa",
                "Tài khoản của bạn đã bị khóa do nhiều lần đăng nhập thất bại. Vui lòng thử lại sau.");
        }

        if (!result.Succeeded)
        {
            throw new BadRequestException("Đăng nhập thất bại",
                "Thông tin đăng nhập không chính xác. Vui lòng kiểm tra lại email và mật khẩu.");
        }
    }

    public async Task GoogleLoginAsync(GoogleLoginRequest request, bool useCookies)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken,
                new GoogleJsonWebSignature.ValidationSettings { Audience = [configuration["GoogleClientId"]] });

            if (!payload.EmailVerified)
            {
                throw new BadRequestException("Đăng Nhập Bằng Google Thất Bại",
                    "Email của tài khoản Google chưa được xác minh.");
            }

            var user = await userManager.FindByLoginAsync(LoginProvider.Google, payload.Subject);
            if (user == null)
            {
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
                        throw new ValidationException(GetErrors(result));
                    }

                    result = await userManager.AddToRoleAsync(user, UserRole.Customer);
                    if (!result.Succeeded)
                    {
                        throw new ValidationException(GetErrors(result));
                    }

                    result = await userManager.AddLoginAsync(user,
                        new UserLoginInfo(LoginProvider.Google, payload.Subject,
                            LoginProvider.Google));
                    if (!result.Succeeded)
                    {
                        throw new ValidationException(GetErrors(result));
                    }
                }
                else
                {
                    var result = await userManager.AddLoginAsync(user,
                        new UserLoginInfo(LoginProvider.Google, payload.Subject,
                            LoginProvider.Google));
                    if (!result.Succeeded)
                    {
                        throw new ValidationException(GetErrors(result));
                    }

                    if (!user.EmailConfirmed)
                    {
                        user.EmailConfirmed = true;
                        result = await userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            throw new ValidationException(GetErrors(result));
                        }
                    }
                }
            }

            signInManager.AuthenticationScheme = useCookies
                ? IdentityConstants.ApplicationScheme
                : IdentityConstants.BearerScheme;
            await signInManager.SignInAsync(user, useCookies);
        }
        catch (InvalidJwtException)
        {
            throw new BadRequestException("Đăng Nhập Bằng Google Thất Bại",
                "Đăng nhập bằng Google không thành công. Vui lòng thử lại.");
        }
    }

    public async Task SendConfirmationEmailAsync()
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("Người dùng chưa đăng nhập",
                "Bạn phải đăng nhập để thực hiện hành động này.");
        }

        if (await userManager.FindByIdAsync(userId) is not { } appUser)
        {
            throw new NotFoundException("Người dùng không tồn tại",
                "Không tìm thấy người dùng với thông tin đã cung cấp.");
        }

        if (appUser.EmailConfirmed)
        {
            throw new BadRequestException("Email đã được xác nhận",
                "Email này đã được xác nhận trước đó.");
        }

        var clientUrl = configuration["ClientUrl"] ??
                        throw new InvalidOperationException("ClientUrl is not configured.");
        var confirmEmailPath = configuration["ConfirmEmailPath"] ??
                               throw new InvalidOperationException("ConfirmEmailPath is not configured.");

        if (string.IsNullOrEmpty(appUser.Email))
        {
            throw new InvalidOperationException("User email must not be null or empty.");
        }

        var encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(appUser.Email));
        var encodedCode = WebEncoders.Base64UrlEncode(
            Encoding.UTF8.GetBytes(await userManager.GenerateEmailConfirmationTokenAsync(appUser)));

        var confirmEmailUrl =
            $"{clientUrl.TrimEnd('/')}/{confirmEmailPath.TrimStart('/')}?email={encodedEmail}&code={encodedCode}";

        await SendEmailConfirmationLinkAsync(appUser.Email, HtmlEncoder.Default.Encode(confirmEmailUrl));
    }

    public async Task ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        var email = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Email));
        if (await userManager.FindByEmailAsync(email) is not { } user)
        {
            throw new BadRequestException("Xác Nhận Email Thất Bại",
                "Xác nhận email không thành công. Vui lòng thử lại.");
        }

        if (user.EmailConfirmed)
        {
            return;
        }

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new BadRequestException("Xác Nhận Email Thất Bại",
                    "Xác nhận email không thành công. Vui lòng thử lại.");
            }
        }
        catch (FormatException)
        {
            throw new BadRequestException("Xác Nhận Email Thất Bại",
                "Xác nhận email không thành công. Vui lòng thử lại.");
        }
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        if (await userManager.FindByEmailAsync(request.Email) is not { } user)
        {
            return;
        }

        var clientUrl = configuration["ClientUrl"] ??
                        throw new InvalidOperationException("ClientUrl is not configured.");
        var resetPasswordPath = configuration["ResetPasswordPath"] ??
                                throw new InvalidOperationException("ResetPasswordPath is not configured.");

        if (string.IsNullOrEmpty(user.Email))
        {
            throw new InvalidOperationException("User email must not be null or empty.");
        }

        var encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Email));
        var encodedCode = WebEncoders.Base64UrlEncode(
            Encoding.UTF8.GetBytes(await userManager.GeneratePasswordResetTokenAsync(user)));

        var resetPasswordUrl =
            $"{clientUrl.TrimEnd('/')}/{resetPasswordPath.TrimStart('/')}?email={encodedEmail}&code={encodedCode}";

        await SendPasswordResetLinkAsync(user.Email!, HtmlEncoder.Default.Encode(resetPasswordUrl));
    }

    public async Task ValidateResetPasswordRequestAsync(ValidateResetPasswordRequest request)
    {
        var email = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Email));
        if (await userManager.FindByEmailAsync(email) is not { } user)
        {
            throw new BadRequestException("Yêu Cầu Đặt Lại Mật Khẩu Không Hợp Lệ",
                "Yêu cầu đặt lại mật khẩu không hợp lệ. Vui lòng thử lại.");
        }

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetCode));
            var result = await userManager.VerifyUserTokenAsync(user,
                userManager.Options.Tokens.PasswordResetTokenProvider, UserManager<AppUser>.ResetPasswordTokenPurpose,
                code);
            if (!result)
            {
                throw new BadRequestException("Yêu Cầu Đặt Lại Mật Khẩu Không Hợp Lệ",
                    "Yêu cầu đặt lại mật khẩu không hợp lệ. Vui lòng thử lại.");
            }
        }
        catch (FormatException)
        {
            throw new BadRequestException("Yêu Cầu Đặt Lại Mật Khẩu Không Hợp Lệ",
                "Yêu cầu đặt lại mật khẩu không hợp lệ. Vui lòng thử lại.");
        }
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        var email = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Email));
        if (await userManager.FindByEmailAsync(email) is not { } user)
        {
            throw new BadRequestException("Đặt Lại Mật Khẩu Thất Bại",
                "Đặt lại mật khẩu không thành công. Vui lòng thử lại.");
        }

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetCode));
            var result = await userManager.ResetPasswordAsync(user, code, request.NewPassword);
            if (!result.Succeeded)
            {
                throw new ValidationException(GetErrors(result));
            }
        }
        catch (FormatException)
        {
            throw new BadRequestException("Đặt Lại Mật Khẩu Thất Bại",
                "Đặt lại mật khẩu không thành công. Vui lòng thử lại.");
        }
    }

    public async Task CookieLogoutAsync() => await signInManager.SignOutAsync();

    public async Task<UserInfoResponse> GetUserInfoAsync()
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("Người dùng chưa đăng nhập",
                "Bạn phải đăng nhập để thực hiện hành động này.");
        }

        if (await userManager.FindByIdAsync(userId) is not { } appUser)
        {
            throw new NotFoundException("Người dùng không tồn tại",
                "Không tìm thấy người dùng với thông tin đã cung cấp.");
        }

        return new UserInfoResponse
        {
            Email = appUser.Email ?? string.Empty,
            EmailConfirmed = appUser.EmailConfirmed,
            HasPassword = !string.IsNullOrEmpty(appUser.PasswordHash),
            Roles = (await userManager.GetRolesAsync(appUser)).ToList()
        };
    }

    public async Task SetPasswordAsync(SetPasswordRequest request)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("Người dùng chưa đăng nhập",
                "Bạn phải đăng nhập để thực hiện hành động này.");
        }

        if (await userManager.FindByIdAsync(userId) is not { } user)
        {
            throw new NotFoundException("Người dùng không tồn tại",
                "Không tìm thấy người dùng với thông tin đã cung cấp.");
        }

        if (!string.IsNullOrEmpty(user.PasswordHash))
        {
            throw new BadRequestException("Tài Khoản Đã Có Mật Khẩu",
                "Tài khoản này đã có mật khẩu. Vui lòng sử dụng chức năng đổi mật khẩu.");
        }

        var result = await userManager.AddPasswordAsync(user, request.NewPassword);
        if (!result.Succeeded)
        {
            throw new ValidationException(GetErrors(result));
        }
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest request)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("Người dùng chưa đăng nhập",
                "Bạn phải đăng nhập để thực hiện hành động này.");
        }

        if (await userManager.FindByIdAsync(userId) is not { } user)
        {
            throw new NotFoundException("Người dùng không tồn tại",
                "Không tìm thấy người dùng với thông tin đã cung cấp.");
        }

        if (string.IsNullOrEmpty(user.PasswordHash))
        {
            throw new BadRequestException("Tài Khoản Chưa Có Mật Khẩu",
                "Tài khoản này chưa có mật khẩu. Vui lòng sử dụng chức năng đặt mật khẩu.");
        }

        if (!await userManager.CheckPasswordAsync(user, request.OldPassword))
        {
            throw new BadRequestException("Đổi Mật Khẩu Thất Bại",
                "Mật khẩu cũ không đúng. Vui lòng thử lại.");
        }

        if (request.OldPassword == request.NewPassword)
        {
            throw new BadRequestException("Đổi Mật Khẩu Thất Bại",
                "Mật khẩu mới phải khác với mật khẩu cũ.");
        }

        var result = await userManager.ChangePasswordAsync(user, request.OldPassword,
            request.NewPassword);

        if (result.Succeeded)
        {
            return;
        }

        var errors = result.Errors.ToList();

        if (errors.Any(e => e.Code == "PasswordTooShort"))
        {
            errors.First(e => e.Code == "PasswordTooShort").Description =
                $"Mật khẩu mới phải có ít nhất {userManager.Options.Password.RequiredLength} ký tự.";
        }

        if (errors.Any(e => e.Code == "PasswordRequiresNonAlphanumeric"))
        {
            errors.First(e => e.Code == "PasswordRequiresNonAlphanumeric").Description =
                "Mật khẩu mới phải chứa ít nhất một ký tự không phải chữ hoặc số.";
        }

        if (errors.Any(e => e.Code == "PasswordRequiresDigit"))
        {
            errors.First(e => e.Code == "PasswordRequiresDigit").Description =
                "Mật khẩu mới phải chứa ít nhất một chữ số ('0'-'9').";
        }

        if (errors.Any(e => e.Code == "PasswordRequiresLower"))
        {
            errors.First(e => e.Code == "PasswordRequiresLower").Description =
                "Mật khẩu mới phải chứa ít nhất một chữ cái viết thường ('a'-'z').";
        }

        if (errors.Any(e => e.Code == "PasswordRequiresUpper"))
        {
            errors.First(e => e.Code == "PasswordRequiresUpper").Description =
                "Mật khẩu mới phải chứa ít nhất một chữ cái viết hoa ('A'-'Z').";
        }

        throw new ValidationException(GetErrors(IdentityResult.Failed(errors.ToArray())));
    }

    private static Dictionary<string, string[]> GetErrors(IdentityResult result) =>
        result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description });

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
}