// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;

namespace Application.Features.Identity.DTOs;

public class ConfirmEmailRequest
{
    [Required(ErrorMessage = "Email là bắt buộc.")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "Code không hợp lệ.")]
    public required string Code { get; init; }
}