using Application.Features.Brands.Queries.GetBrands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("brands")]
public class BrandController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBrands([FromQuery] GetBrandsQuery query) =>
        Ok(await sender.Send(query));
}