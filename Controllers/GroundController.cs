using CricketGroundBookingApi.DTOs.Grounds;
using CricketGroundBookingApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CricketGroundBookingApi.Controllers;

[ApiController]
[Route("api/v1/grounds")]
public class GroundController : ControllerBase
{
    private readonly IGroundService _groundService;

    public GroundController(IGroundService groundService)
    {
        _groundService = groundService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var grounds = await _groundService.GetAllAsync();

        return Ok(new
        {
            success = true,
            data = grounds
        });
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var ground = await _groundService.GetByIdAsync(id);

        if (ground is null)
        {
            return NotFound(new
            {
                success = false,
                message = "Ground not found"
            });
        }

        return Ok(new
        {
            success = true,
            data = ground
        });
    }

    // ADMIN: Create ground
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateGroundRequest request)
    {


        var ground = await _groundService.CreateAsync(request);

        return Ok(new
        {
            success = true,
            message = "Ground created successfully",
            data = ground
        });
    }

    // ADMIN: Update ground
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id,
        UpdateGroundRequest request
    )
    {
        var ground = await _groundService.UpdateAsync(id, request);

        if(ground is null)
        {
            return NotFound(new
            {
                success = false,
                message = "Ground not found"
            });
        }

        return Ok(new
        {
            success = true,
            message = "Ground updated successfully",
            data = ground
        });
    }

    // ADMIN: Delete ground
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = await _groundService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                success = false,
                message = "Ground not found"
            });
        }

        return Ok(new
        {
            success = true,
            message = "Ground deleted successfully"
        });
    }
}