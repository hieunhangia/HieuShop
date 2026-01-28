using Application.Features.Addresses.Queries.GetProvinces;
using Application.Features.Addresses.Queries.GetWardsByProvinceId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
public class AddressController(ISender sender) : ControllerBase
{
    [HttpGet("/provinces")]
    public async Task<IActionResult> GetProvinces() => Ok(await sender.Send(new GetProvincesQuery()));

    [HttpGet("/provinces/{provinceId:int}/wards")]
    public async Task<IActionResult> GetWardsByProvinceId(int provinceId) =>
        Ok(await sender.Send(new GetWardByProvinceIdQuery { ProvinceId = provinceId }));
}