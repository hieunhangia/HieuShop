// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;

namespace Application.Features.Identity.DTOs;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "RefreshToken là bắt buộc.")]
    public required string RefreshToken { get; init; }
}