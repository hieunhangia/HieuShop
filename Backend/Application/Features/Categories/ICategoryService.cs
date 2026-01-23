using Application.Features.Categories.DTOs;

namespace Application.Features.Categories;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> QueryCategoriesAsync(GetCategoriesQuery query);
}