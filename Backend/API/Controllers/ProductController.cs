using Application.Features.Products.Queries.GetProductBySlug;
using Application.Features.Products.Queries.SearchProductsPagedSorted;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("products")]
public class ProductController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> SearchProductsPagedSorted([FromQuery] SearchProductsPagedSortedQuery query) =>
        Ok(await sender.Send(query));

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetProductBySlug([FromRoute] string slug) =>
        Ok(await sender.Send(new GetProductBySlugQuery { Slug = slug }));
}