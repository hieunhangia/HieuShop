using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Features.Products.DTOs;
using Application.Features.Products.Enums;
using Domain.Entities.Products;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.Features.Products;

public class ProductService(
    IUnitOfWork unitOfWork,
    ProductMapper mapper,
    IValidator<GetProductsQuery> getProductsQueryValidator) : IProductService
{
    public async Task<PagedAndSortedResult<ProductSummaryResponse>> QueryActiveProductsAsync(
        GetProductsQuery query)
    {
        var validationResult = await getProductsQueryValidator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var pageIndex = query.PageIndex ?? 1;
        var pageSize = query.PageSize ?? 10;
        query.SortColumn ??= ProductSortColumn.CreatedAt;
        var sortColumn = query.SortColumn switch
        {
            ProductSortColumn.Name => nameof(Product.Name),
            ProductSortColumn.Price =>
                $"{nameof(Product.DefaultProductVariant)}.{nameof(Product.DefaultProductVariant.Price)}",
            _ => nameof(Product.CreatedAt)
        };
        var sortDirection = query.SortDirection ?? SortDirection.Asc;

        var pagedProducts =
            await unitOfWork.Products.QueryActiveProductsReadOnlyAsync(query.SearchText, pageIndex, pageSize,
                sortColumn, sortDirection);
        return new PagedAndSortedResult<ProductSummaryResponse>(mapper.MapToSummaryList(pagedProducts.Products),
            pagedProducts.TotalCount, pageIndex, pageSize, sortColumn, sortDirection);
    }

    public async Task<ProductDetailResponse?> GetProductBySlugAsync(string slug)
    {
        var product = await unitOfWork.Products.GetBySlugWithDetailsReadOnlyAsync(slug);
        if (product == null)
        {
            throw new NotFoundException("Không tìm thấy sản phẩm với slug đã cho.");
        }

        return new ProductDetailResponse
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
                    Id = product.Brand!.Id,
                    Name = product.Brand.Name,
                    Slug = product.Brand.Slug
                }
                : null,
            Categories = product.Categories!
                .Where(category => category.IsActive)
                .Select(category => new CategoryResponse
                {
                    Id = category.Id,
                    Name = category.Name,
                    Slug = category.Slug
                }).ToList(),
            Options = product.ProductOptions!
                .Where(option => option.IsActive)
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