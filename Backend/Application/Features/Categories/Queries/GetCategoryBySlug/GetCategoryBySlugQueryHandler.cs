using Application.Common.Exceptions;
using Application.Features.Categories.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategoryBySlug;

public class GetCategoryBySlugQueryHandler(IUnitOfWork unitOfWork, CategoryMapper mapper)
    : IRequestHandler<GetCategoryBySlugQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryBySlugQuery request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.Categories.GetBySlugAsync(request.Slug!);
        return category != null
            ? mapper.MapToDto(category)
            : throw new NotFoundException($"Không tìm thấy danh mục với slug '{request.Slug}'.");
    }
}