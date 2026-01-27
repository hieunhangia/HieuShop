namespace Application.Features.Carts.DTOs;

public class CartDto
{
    public required IReadOnlyList<CartItemDto> CartItems { get; set; }
    public string? WarningMessage { get; set; }
}