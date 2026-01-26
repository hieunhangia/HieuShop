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
    public async Task<IActionResult> SearchProducts([FromQuery] SearchProductsQuery query) =>
        Ok(await sender.Send(query));

    [HttpGet("/brands/{brandSlug}/products")]
    public async Task<IActionResult> SearchProductsByBrandSlug([FromRoute] string brandSlug,
        [FromQuery] SearchProductsByBrandSlugQuery query)
    {
        query.BrandSlug = brandSlug;
        return Ok(await sender.Send(query));
    }

    [HttpGet("/categories/{categorySlug}/products")]
    public async Task<IActionResult> SearchProductsByCategorySlug([FromRoute] string categorySlug,
        [FromQuery] SearchProductsByCategorySlugQuery query)
    {
        query.CategorySlug = categorySlug;
        return Ok(await sender.Send(query));
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetProductBySlug([FromRoute] string slug) =>
        Ok(await sender.Send(new GetProductBySlugQuery { Slug = slug }));
}