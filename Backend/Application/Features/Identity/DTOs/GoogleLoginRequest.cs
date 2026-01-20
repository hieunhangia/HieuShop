using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Application.Features.Identity.DTOs;

public class GoogleLoginRequest
{
    [Required] public required string IdToken { get; init; }
}