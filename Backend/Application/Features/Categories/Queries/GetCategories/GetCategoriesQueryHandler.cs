using Application.Features.Categories.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler(IUnitOfWork unitOfWork, CategoryMapper mapper)
    : IRequestHandler<GetCategoriesQuery, IReadOnlyList<CategoryDto>>
{
    public async Task<IReadOnlyList<CategoryDto>>
        Handle(GetCategoriesQuery request, CancellationToken cancellationToken) =>
        mapper.MapToDtoList(await unitOfWork.Categories.GetTopActiveCategoriesReadOnlyAsync(request.Top ?? 10));
}