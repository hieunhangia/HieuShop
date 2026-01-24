using MediatR;

namespace Application.Features.Products.Queries.GetProductBySlug;

public class GetProductBySlugQuery : IRequest<ProductDto>
{
    public string? Slug { get; set; }
}