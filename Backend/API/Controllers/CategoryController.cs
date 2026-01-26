using Application.Features.Categories.Queries.GetCategories;
using Application.Features.Categories.Queries.GetCategoryBySlug;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("categories")]
public class CategoryController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesQuery query) =>
        Ok(await sender.Send(query));

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetCategoryBySlug(string slug) =>
        Ok(await sender.Send(new GetCategoryBySlugQuery { Slug = slug }));
}