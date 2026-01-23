using Application.Features.Brands;
using Application.Features.Brands.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("brands")]
public class BrandController(IBrandService brandService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBrands([FromQuery] GetBrandsQuery query) =>
        Ok(await brandService.QueryBrandsAsync(query));
}