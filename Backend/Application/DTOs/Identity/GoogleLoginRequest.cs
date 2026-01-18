using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Application.DTOs.Identity;

public class GoogleLoginRequest
{
    [Required] public required string IdToken { get; init; }
}