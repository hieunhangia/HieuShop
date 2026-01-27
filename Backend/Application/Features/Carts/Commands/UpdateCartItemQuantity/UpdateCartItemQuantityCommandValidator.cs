using FluentValidation;

namespace Application.Features.Carts.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommandValidator : AbstractValidator<UpdateCartItemQuantityCommand>
{
    public UpdateCartItemQuantityCommandValidator()
    {
        RuleFor(x => x.ProductVariantId)
            .NotEmpty().WithMessage("ProductVariantId là bắt buộc.");
        RuleFor(x => x.NewQuantity)
            .GreaterThan(0).WithMessage("NewQuantity phải lớn hơn 0.");
    }
}