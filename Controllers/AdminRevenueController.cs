using CricketGroundBookingApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CricketGroundBookingApi.Controllers;

[ApiController]
[Route("api/v1/admin/revenue")]
[Authorize(Roles = "Admin")]
public class AdminRevenueController : ControllerBase
{
    private readonly IAdminRevenueService _adminRevenueService;

    public AdminRevenueController(IAdminRevenueService adminRevenueService)
    {
        _adminRevenueService = adminRevenueService;
    }

    /// <summary>
    /// Returns revenue summary metrics for the admin dashboard.
    /// </summary>
    /// <param name="startDate">Optional start date filter in yyyy-MM-dd format.</param>
    /// <param name="endDate">Optional end date filter in yyyy-MM-dd format.</param>
    /// <param name="paymentStatus">Optional payment status filter.</param>
    [HttpGet("summary")]
    public async Task<IActionResult> GetRevenueSummary(
        [FromQuery] string? startDate = null,
        [FromQuery] string? endDate = null,
        [FromQuery] string? paymentStatus = null
    )
    {
        var dateValidationResult = ValidateDateRange(startDate, endDate, out var parsedStartDate, out var parsedEndDate);

        if (dateValidationResult is not null)
        {
            return dateValidationResult;
        }

        var response = await _adminRevenueService.GetRevenueSummaryAsync(
            parsedStartDate,
            parsedEndDate,
            paymentStatus
        );

        return Ok(new
        {
            success = true,
            data = response
        });
    }

    /// <summary>
    /// Returns revenue grouped by booking slot type.
    /// </summary>
    /// <param name="startDate">Optional start date filter in yyyy-MM-dd format.</param>
    /// <param name="endDate">Optional end date filter in yyyy-MM-dd format.</param>
    [HttpGet("by-slot-type")]
    public async Task<IActionResult> GetRevenueBySlotType(
        [FromQuery] string? startDate = null,
        [FromQuery] string? endDate = null
    )
    {
        var dateValidationResult = ValidateDateRange(startDate, endDate, out var parsedStartDate, out var parsedEndDate);

        if (dateValidationResult is not null)
        {
            return dateValidationResult;
        }

        var response = await _adminRevenueService.GetRevenueBySlotTypeAsync(
            parsedStartDate,
            parsedEndDate
        );

        return Ok(new
        {
            success = true,
            data = response
        });
    }

    /// <summary>
    /// Returns revenue grouped by ground.
    /// </summary>
    /// <param name="startDate">Optional start date filter in yyyy-MM-dd format.</param>
    /// <param name="endDate">Optional end date filter in yyyy-MM-dd format.</param>
    /// <param name="limit">Optional top N grounds limit.</param>
    [HttpGet("by-ground")]
    public async Task<IActionResult> GetRevenueByGround(
        [FromQuery] string? startDate = null,
        [FromQuery] string? endDate = null,
        [FromQuery] int limit = 10
    )
    {
        if (limit < 1)
        {
            return BadRequest(new
            {
                success = false,
                message = "limit must be greater than 0"
            });
        }

        var dateValidationResult = ValidateDateRange(startDate, endDate, out var parsedStartDate, out var parsedEndDate);

        if (dateValidationResult is not null)
        {
            return dateValidationResult;
        }

        var response = await _adminRevenueService.GetRevenueByGroundAsync(
            parsedStartDate,
            parsedEndDate,
            limit
        );

        return Ok(new
        {
            success = true,
            data = response
        });
    }

    /// <summary>
    /// Returns revenue grouped by addon.
    /// </summary>
    /// <param name="startDate">Optional start date filter in yyyy-MM-dd format.</param>
    /// <param name="endDate">Optional end date filter in yyyy-MM-dd format.</param>
    [HttpGet("by-addon")]
    public async Task<IActionResult> GetRevenueByAddon(
        [FromQuery] string? startDate = null,
        [FromQuery] string? endDate = null
    )
    {
        var dateValidationResult = ValidateDateRange(startDate, endDate, out var parsedStartDate, out var parsedEndDate);

        if (dateValidationResult is not null)
        {
            return dateValidationResult;
        }

        var response = await _adminRevenueService.GetRevenueByAddonAsync(
            parsedStartDate,
            parsedEndDate
        );

        return Ok(new
        {
            success = true,
            data = response
        });
    }

    private IActionResult? ValidateDateRange(
        string? startDate,
        string? endDate,
        out DateOnly? parsedStartDate,
        out DateOnly? parsedEndDate
    )
    {
        parsedStartDate = null;
        parsedEndDate = null;

        if (!string.IsNullOrWhiteSpace(startDate))
        {
            if (!DateOnly.TryParse(startDate, out var start))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid startDate format. Expected yyyy-MM-dd"
                });
            }

            parsedStartDate = start;
        }

        if (!string.IsNullOrWhiteSpace(endDate))
        {
            if (!DateOnly.TryParse(endDate, out var end))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid endDate format. Expected yyyy-MM-dd"
                });
            }

            parsedEndDate = end;
        }

        if (parsedStartDate.HasValue && parsedEndDate.HasValue && parsedStartDate > parsedEndDate)
        {
            return BadRequest(new
            {
                success = false,
                message = "startDate cannot be later than endDate"
            });
        }

        return null;
    }
}
