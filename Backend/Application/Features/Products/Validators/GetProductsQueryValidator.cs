using Application.Features.Products.DTOs;
using FluentValidation;

namespace Application.Features.Products.Validators;

public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0).WithMessage("Trang phải lớn hơn 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Kích thước trang phải lớn hơn 0.")
            .LessThan(51).WithMessage("Kích thước trang không được vượt quá 50.");
    }
}