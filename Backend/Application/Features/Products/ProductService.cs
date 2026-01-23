using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Features.Products.DTOs;
using Application.Features.Products.Enums;
using Domain.Entities.Products;
using Domain.Interfaces;
using FluentValidation;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.Features.Products;

public class ProductService(
    IUnitOfWork unitOfWork,
    IValidator<GetProductsQuery> getProductQueryValidator) : IProductService
{
    public async Task<PagedAndSortedResult<ProductSummaryResponse>> QueryActiveProductsAsync(
        GetProductsQuery query)
    {
        var validationResult = await getProductQueryValidator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var sortColumn = query.SortColumn switch
        {
            ProductSortColumn.Name => nameof(Product.Name),
            ProductSortColumn.Price =>
                $"{nameof(Product.DefaultProductVariant)}.{nameof(Product.DefaultProductVariant.Price)}",
            _ => nameof(Product.CreatedAt)
        };

        var pagedProducts = await unitOfWork.Products.QueryActiveProductsReadOnlyAsync(query.SearchText,
            query.PageIndex, query.PageSize, sortColumn, query.SortDirection);
        return new PagedAndSortedResult<ProductSummaryResponse>(
            pagedProducts.Products.Select(product => new ProductSummaryResponse
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Price = product.DefaultProductVariant!.Price,
                SalePrice = product.DefaultProductVariant.SalePrice
            }).ToList(),
            pagedProducts.TotalCount, query.PageIndex, query.PageSize, query.SortDirection);
    }

    public async Task<ProductDetailResponse?> GetProductBySlugAsync(string slug)
    {
        var product = await unitOfWork.Products.GetBySlugWithDetailsAsync(slug);
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