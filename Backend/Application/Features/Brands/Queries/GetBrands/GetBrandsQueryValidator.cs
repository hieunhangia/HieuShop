using FluentValidation;

namespace Application.Features.Brands.Queries.GetBrands;

public class GetBrandsQueryValidator : AbstractValidator<GetBrandsQuery>
{
    public GetBrandsQueryValidator() =>
        RuleFor(x => x.Top)
            .GreaterThan(0).WithMessage("Số lượng thương hiệu trả về phải lớn hơn 0.")
            .LessThan(20).WithMessage("Số lượng thương hiệu trả về phải nhỏ hơn 20.");
}