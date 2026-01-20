using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Application.Features.Identity.DTOs;

public class SetPasswordRequest
{
    [Required] public required string NewPassword { get; init; }
}