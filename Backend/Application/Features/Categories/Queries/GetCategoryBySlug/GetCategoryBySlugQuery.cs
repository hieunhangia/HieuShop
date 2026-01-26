using Application.Features.Categories.DTOs;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategoryBySlug;

public class GetCategoryBySlugQuery : IRequest<CategoryDto>
{
    public string? Slug { get; set; }
}