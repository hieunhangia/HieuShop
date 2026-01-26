using FluentValidation;

namespace Application.Features.Products.Queries.SearchProductsByCategorySlug;

public class SearchProductsByCategorySlugQueryValidator : AbstractValidator<SearchProductsByCategorySlugQuery>
{
    public SearchProductsByCategorySlugQueryValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0).WithMessage("Trang phải lớn hơn 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Kích thước trang phải lớn hơn 0.")
            .LessThan(51).WithMessage("Kích thước trang không được vượt quá 50.");
    }
}