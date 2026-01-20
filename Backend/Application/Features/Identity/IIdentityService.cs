using Application.Features.Identity.DTOs;

namespace Application.Features.Identity;

public interface IIdentityService
{
    Task<string> RegisterAsync(RegisterRequest request);
    Task LoginAsync(LoginRequest request, bool useCookies);
    Task GoogleLoginAsync(GoogleLoginRequest request, bool useCookies);
    Task SendConfirmationEmailAsync();
    Task ConfirmEmailAsync(ConfirmEmailRequest request);
    Task ForgotPasswordAsync(ForgotPasswordRequest request);
    Task ValidateResetPasswordRequestAsync(ValidateResetPasswordRequest request);
    Task ResetPasswordAsync(ResetPasswordRequest request);
    Task CookieLogoutAsync();
    Task<UserInfoResponse> GetUserInfoAsync();
    Task SetPasswordAsync(SetPasswordRequest request);
    Task ChangePasswordAsync(ChangePasswordRequest request);
}