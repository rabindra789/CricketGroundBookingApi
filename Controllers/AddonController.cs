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
    public async Task<IActionResult> GetAll([FromQuery] string? sportType = null)
    {
        var addons = await _addonService.GetAllAsync(sportType);

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

            return StatusCode(StatusCodes.Status201Created, new
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

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] CreateAddonRequest request
    )
    {
        var addon = await _addonService.UpdateAsync(id, request);

        if (addon is null)
        {
            return NotFound(new
            {
                success = false,
                message = "Addon not found"
            });
        }

        return Ok(new
        {
            success = true,
            data = addon
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = await _addonService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                success = false,
                message = "Addon not found"
            });
        }

        return NoContent();
    }
}
