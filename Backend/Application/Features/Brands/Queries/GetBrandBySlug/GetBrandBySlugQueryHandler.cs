using Application.Common.Exceptions;
using Application.Features.Brands.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Brands.Queries.GetBrandBySlug;

public class GetBrandBySlugQueryHandler(IUnitOfWork unitOfWork, BrandMapper mapper)
    : IRequestHandler<GetBrandBySlugQuery, BrandDto>
{
    public async Task<BrandDto> Handle(GetBrandBySlugQuery request, CancellationToken cancellationToken)
    {
        var brand = await unitOfWork.Brands.GetBySlugAsync(request.Slug ?? string.Empty);
        return brand != null
            ? mapper.MapToDto(brand)
            : throw new NotFoundException($"Không tìm thấy thương hiệu với slug: {request.Slug}");
    }
}