using CricketGroundBookingApi.DTOs.Slots;
using CricketGroundBookingApi.Interfaces;
using CricketGroundBookingApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CricketGroundBookingApi.Controllers;

[ApiController]
[Route("api/v1/slots")]
public class SlotController : ControllerBase
{
    private readonly ISlotService _slotService;

    public SlotController(ISlotService slotService)
    {
        _slotService = slotService;
    }

    // PUBLIC: Get all slots
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var slots = await _slotService.GetAllAsync();

        return Ok(new
        {
            success = true,
            data = slots
        });
    }

    // PUBLIC: Get slots bu ground
    [HttpGet("ground/{groundId:long}")]
    public async Task<IActionResult> GetByGround(long groundId)
    {
        var slots = await _slotService.GetByGroundIdAsync(groundId);

        return Ok(new
        {
            success = true,
            data = slots
        });
    }

    // PUBLIC: Get slot by id
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var slot = await _slotService.GetByIdAsync(id);

        if (slot is null)
        {
            return NotFound(new
            {
                success = false,
                message = "Slot not found"
            });
        }

        return Ok(new
        {
            success = true,
            data = slot
        });
    }

    // ADMIN: Create slot
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateSlotRequest request)
    {
        var slot = await _slotService.CreateAsync(request);

        return Ok(new
        {
            success = true,
            message = "Slot created successfully",
            data = slot
        });
    }

    // ADMIN: Update slot
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id,
        UpdateSlotRequest request
    )
    {
        var slot = await _slotService.UpdateAsync(id, request);

        if (slot is null)
        {
            return NotFound(new
            {
                success = false,
                message = "Slot not found"
            });
        }

        return Ok(new
        {
            success = true,
            message = "Slot updated successfully",
            data = slot
        });
    }

    // ADMIN: Delete slot
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = await _slotService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                success = false,
                message = "Slot not found"
            });
        }

        return Ok(new
        {
            success = true,
            message = "Slot deleted successfully"
        });
    }
}