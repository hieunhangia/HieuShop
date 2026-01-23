using Application.Features.Products;
using Application.Features.Products.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("products")]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query) =>
        Ok(await productService.QueryActiveProductsAsync(query));

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetProductBySlug([FromRoute] string slug) =>
        Ok(await productService.GetProductBySlugAsync(slug));
}