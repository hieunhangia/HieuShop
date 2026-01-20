using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Application.Features.Identity.DTOs;

public class ValidateResetPasswordRequest
{
    [Required] public required string Email { get; init; }
    [Required] public required string ResetCode { get; init; }
}