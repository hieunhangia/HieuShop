using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Application.DTOs.Identity;

public class ValidateResetPasswordRequest
{
    [Required] public required string Email { get; init; }
    [Required] public required string ResetCode { get; init; }
}