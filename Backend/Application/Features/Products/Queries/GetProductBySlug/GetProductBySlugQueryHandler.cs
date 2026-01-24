using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries.GetProductBySlug;

public class GetProductBySlugQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProductBySlugQuery, ProductDetailDto>
{
    public async Task<ProductDetailDto> Handle(GetProductBySlugQuery request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetBySlugWithDetailsReadOnlyAsync(request.Slug ?? string.Empty);
        if (product == null)
        {
            throw new NotFoundException("Không tìm thấy sản phẩm với slug đã cho.");
        }

        return new ProductDetailDto
        {
            Id = product.Id,
            Name = product.Name,
            Slug = product.Slug,
            Description = product.Description,
            Price = product.DefaultProductVariant!.Price,
            SalePrice = product.DefaultProductVariant.SalePrice,
            ImageUrls = product.ProductImages!.Select(pi => pi.ImageUrl).ToList(),
            Brand = product.Brand!.IsActive
                ? new BrandResponse
                {
                    Id = product.Brand.Id,
                    Name = product.Brand.Name,
                    Slug = product.Brand.Slug
                }
                : null,
            Categories = product.Categories!
                .Select(category => new CategoryResponse
                {
                    Id = category.Id,
                    Name = category.Name,
                    Slug = category.Slug
                }).ToList(),
            Options = product.ProductOptions!
                .Select(option => new ProductOptionResponse
                {
                    Name = option.Name,
                    Values = option.ProductOptionValues!.Select(value => new ProductOptionValueResponse
                    {
                        Value = value.Value
                    }).ToList()
                }).ToList()
        };
    }
}