using Application.Features.Categories.DTOs;
using FluentValidation;

namespace Application.Features.Categories.Validators;

public class GetCategoriesQueryValidator : AbstractValidator<GetCategoriesQuery>
{
    public GetCategoriesQueryValidator() =>
        RuleFor(x => x.Top)
            .GreaterThan(0).WithMessage("Số lượng danh mục trả về phải lớn hơn 0.")
            .LessThanOrEqualTo(10).WithMessage("Số lượng danh mục trả về không được vượt quá 10.");
}