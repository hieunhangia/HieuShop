using Application.Features.Categories.DTOs;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Features.Categories;

public class CategoryService(IUnitOfWork unitOfWork, IValidator<GetCategoriesQuery> getCategoriesQueryValidator)
    : ICategoryService
{
    public async Task<IEnumerable<CategoryResponse>> QueryCategoriesAsync(GetCategoriesQuery query)
    {
        var validationResult = await getCategoriesQueryValidator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        return (await unitOfWork.Categories.QueryActiveCategoriesReadOnlyAsync(query.Top))
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                ImageUrl = c.ImageUrl
            });
    }
}