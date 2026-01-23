using Application.Features.Brands.DTOs;
using FluentValidation;

namespace Application.Features.Brands.Validators;

public class GetBrandsQueryValidator : AbstractValidator<GetBrandsQuery>
{
    public GetBrandsQueryValidator() =>
        RuleFor(x => x.Top)
            .GreaterThan(0).WithMessage("Số lượng thương hiệu trả về phải lớn hơn 0.")
            .LessThan(10).WithMessage("Số lượng thương hiệu trả về phải nhỏ hơn 10.");
}