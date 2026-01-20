// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;

namespace Application.Features.Identity.DTOs;

public class ResetPasswordRequest
{
    [Required(ErrorMessage = "Email là bắt buộc.")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "ResetCode là bắt buộc.")]
    public required string ResetCode { get; init; }

    [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
    public required string NewPassword { get; init; }
}