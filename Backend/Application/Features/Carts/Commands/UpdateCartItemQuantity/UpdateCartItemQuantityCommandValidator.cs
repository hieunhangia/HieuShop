using FluentValidation;

namespace Application.Features.Carts.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommandValidator : AbstractValidator<UpdateCartItemQuantityCommand>
{
    public UpdateCartItemQuantityCommandValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity phải lớn hơn 0.");
    }
}