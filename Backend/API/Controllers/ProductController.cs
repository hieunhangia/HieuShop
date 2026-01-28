using Application.Features.Products.DTOs;
using Application.Features.Products.Queries.GetProductBySlug;
using Application.Features.Products.Queries.SearchProducts;
using Application.Features.Products.Queries.SearchProductsByBrandSlug;
using Application.Features.Products.Queries.SearchProductsByCategorySlug;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("products")]
public class ProductController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> SearchProducts([FromQuery] SearchProductsRequest request) =>
        Ok(await sender.Send(new SearchProductsQuery
        {
            SearchText = request.SearchText,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            SortColumn = request.SortColumn,
            SortDirection = request.SortDirection
        }));

    [HttpGet("/brands/{brandSlug}/products")]
    public async Task<IActionResult> SearchProductsByBrandSlug([FromRoute] string brandSlug,
        [FromQuery] SearchProductsRequest request) =>
        Ok(await sender.Send(new SearchProductsByBrandSlugQuery
        {
            BrandSlug = brandSlug,
            SearchText = request.SearchText,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            SortColumn = request.SortColumn,
            SortDirection = request.SortDirection
        }));

    [HttpGet("/categories/{categorySlug}/products")]
    public async Task<IActionResult> SearchProductsByCategorySlug([FromRoute] string categorySlug,
        [FromQuery] SearchProductsRequest request) =>
        Ok(await sender.Send(new SearchProductsByCategorySlugQuery
        {
            CategorySlug = categorySlug,
            SearchText = request.SearchText,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            SortColumn = request.SortColumn,
            SortDirection = request.SortDirection
        }));

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetProductBySlug([FromRoute] string slug) =>
        Ok(await sender.Send(new GetProductBySlugQuery { Slug = slug }));
}