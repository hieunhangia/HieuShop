using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries.GetProductBySlug;

public class GetProductBySlugQueryHandler(IUnitOfWork unitOfWork, ProductMapper mapper)
    : IRequestHandler<GetProductBySlugQuery, ProductDetailDto>
{
    public async Task<ProductDetailDto> Handle(GetProductBySlugQuery request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetBySlugWithDetailsReadOnlyAsync(request.Slug ?? string.Empty);
        return product == null
            ? throw new NotFoundException("Không tìm thấy sản phẩm với slug đã cho.")
            : mapper.MapToDetail(product);
    }
}