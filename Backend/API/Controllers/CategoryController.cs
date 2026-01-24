using Application.Features.Categories.Queries.GetCategories;
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
}