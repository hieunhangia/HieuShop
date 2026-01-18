using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Application.DTOs.Identity;

public class SetPasswordRequest
{
    [Required] public required string NewPassword { get; init; }
}