using Application.Features.Categories;
using Application.Features.Categories.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("categories")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesQuery query) =>
        Ok(await categoryService.QueryCategoriesAsync(query));
}