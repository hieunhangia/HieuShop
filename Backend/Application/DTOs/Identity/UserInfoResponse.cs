namespace Application.DTOs.Identity;

// ReSharper disable UnusedAutoPropertyAccessor.Global
public class UserInfoResponse
{
    public required string Email { get; init; }
    public required bool EmailConfirmed { get; init; }
    public required bool HasPassword { get; init; }
    public required IEnumerable<string> Roles { get; init; }
}