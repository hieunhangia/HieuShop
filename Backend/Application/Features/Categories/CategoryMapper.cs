using Application.Features.Categories.DTOs;
using Domain.Entities.Products;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Categories;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CategoryMapper
{
    public partial CategoryDto MapToDto(Category category);

    public partial IReadOnlyList<CategoryDto> MapToDtoList(IEnumerable<Category> categories);
}