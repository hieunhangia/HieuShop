using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Application.DTOs.Identity;

public class ChangePasswordRequest
{
    [Required] public required string OldPassword { get; init; }
    [Required] public required string NewPassword { get; init; }
}