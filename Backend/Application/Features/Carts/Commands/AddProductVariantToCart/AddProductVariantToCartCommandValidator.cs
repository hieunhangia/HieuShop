using FluentValidation;

namespace Application.Features.Carts.Commands.AddProductVariantToCart;

public class AddProductVariantToCartCommandValidator : AbstractValidator<AddProductVariantToCartCommand>
{
    public AddProductVariantToCartCommandValidator()
    {
        RuleFor(x => x.ProductVariantId)
            .NotEmpty().WithMessage("ProductVariantId là bắt buộc.");
    }
}