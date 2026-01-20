// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;

namespace Application.Features.Identity.DTOs;

public class RegisterRequest
{
    [Required(ErrorMessage = "Email là bắt buộc.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
    public required string Password { get; init; }
}