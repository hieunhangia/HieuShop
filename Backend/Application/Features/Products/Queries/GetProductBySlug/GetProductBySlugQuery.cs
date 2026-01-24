using MediatR;

namespace Application.Features.Products.Queries.GetProductBySlug;

public class GetProductBySlugQuery : IRequest<ProductDetailDto>
{
    public string? Slug { get; set; }
}