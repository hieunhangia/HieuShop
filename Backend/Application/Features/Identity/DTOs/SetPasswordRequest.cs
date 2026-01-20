// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;

namespace Application.Features.Identity.DTOs;

public class SetPasswordRequest
{
    [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
    public required string NewPassword { get; init; }
}