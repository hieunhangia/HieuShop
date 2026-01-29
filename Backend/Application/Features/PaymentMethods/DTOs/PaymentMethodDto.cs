namespace Application.Features.PaymentMethods.DTOs;

public class PaymentMethodDto
{
    public required Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}