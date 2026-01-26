using Application.Features.Brands.DTOs;
using MediatR;

namespace Application.Features.Brands.Queries.GetBrandBySlug;

public class GetBrandBySlugQuery : IRequest<BrandDto>
{
    public string? Slug { get; set; }
}