using CricketGroundBookingApi.DTOs.Addons;
using CricketGroundBookingApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CricketGroundBookingApi.Controllers;

[ApiController]
[Route("api/v1/addons")]
public class AddonController : ControllerBase
{
    private readonly IAddonService _addonService;

    public AddonController(IAddonService addonService)
    {
        _addonService = addonService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var addons = await _addonService.GetAllAsync();

        return Ok(new
        {
            success = true,
            data = addons
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAddonRequest request
    )
    {
        try
        {
            var addon = await _addonService.CreateAsync(request);

            return Ok(new
            {
                success = true,
                data = addon
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }
}