// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;

namespace Application.Features.Identity.DTOs;

public class ChangePasswordRequest
{
    [Required(ErrorMessage = "Mật khẩu cũ là bắt buộc.")]
    public required string OldPassword { get; init; }

    [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
    public required string NewPassword { get; init; }
}