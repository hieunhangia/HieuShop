using Application.Features.Products.DTOs;
using Application.Features.Products.Queries.GetProductBySlug;
using Application.Features.Products.Queries.SearchProductsPagedSorted;
using Application.Features.Products.Queries.SearchProductsPagedSortedBySlug;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("products")]
public class ProductController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> SearchProductsPagedSorted([FromQuery] SearchProductsPagedSortedRequest query)
    {
        var mappedQuery = new SearchProductsPagedSortedQuery
        {
            SearchText = query.SearchText,
            PageIndex = query.PageIndex,
            PageSize = query.PageSize,
            SortColumn = query.SortColumn,
            SortDirection = query.SortDirection
        };
        return Ok(await sender.Send(mappedQuery));
    }

    [HttpGet("/{slug}/products")]
    public async Task<IActionResult> GetProductsBySlug([FromRoute] string slug,
        [FromQuery] SearchProductsPagedSortedRequest query)
    {
        var mappedQuery = new SearchProductsPagedSortedBySlugQuery
        {
            Slug = slug,
            SearchText = query.SearchText,
            PageIndex = query.PageIndex,
            PageSize = query.PageSize,
            SortColumn = query.SortColumn,
            SortDirection = query.SortDirection
        };
        return Ok(await sender.Send(mappedQuery));
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetProductBySlug([FromRoute] string slug) =>
        Ok(await sender.Send(new GetProductBySlugQuery { Slug = slug }));
}