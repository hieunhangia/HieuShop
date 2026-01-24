using MediatR;

namespace Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQuery : IRequest<IReadOnlyList<CategoryDto>>
{
    public int? Top { get; set; }
}