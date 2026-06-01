using CricketGroundBookingApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CricketGroundBookingApi.Controllers;

[ApiController]
[Route("api/v1/admin/bookings")]
[Authorize(Roles = "Admin")]
public class AdminBookingsController : ControllerBase
{
    private readonly IAdminBookingService _adminBookingService;

    public AdminBookingsController(IAdminBookingService adminBookingService)
    {
        _adminBookingService = adminBookingService;
    }

    /// <summary>
    /// Returns a paged list of bookings for the admin dashboard.
    /// </summary>
    /// <param name="pageNo">Page number. Defaults to 1.</param>
    /// <param name="pageSize">Page size. Defaults to 20.</param>
    /// <param name="status">Optional booking status filter.</param>
    /// <param name="groundId">Optional ground filter.</param>
    /// <param name="startDate">Optional booking date start filter (yyyy-MM-dd).</param>
    /// <param name="endDate">Optional booking date end filter (yyyy-MM-dd).</param>
    [HttpGet]
    public async Task<IActionResult> GetBookings(
        [FromQuery] int pageNo = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? status = null,
        [FromQuery] long? groundId = null,
        [FromQuery] string? startDate = null,
        [FromQuery] string? endDate = null
    )
    {
        if (pageNo < 1)
        {
            return BadRequest(new
            {
                success = false,
                message = "pageNo must be greater than 0"
            });
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest(new
            {
                success = false,
                message = "pageSize must be between 1 and 100"
            });
        }

        DateOnly? parsedStartDate = null;
        DateOnly? parsedEndDate = null;

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

        var response = await _adminBookingService.GetBookingsAsync(
            pageNo,
            pageSize,
            status,
            groundId,
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
    /// Cancels a booking by its identifier.
    /// </summary>
    /// <param name="bookingId">Booking identifier.</param>
    [HttpDelete("{bookingId}")]
    public async Task<IActionResult> CancelBooking(long bookingId)
    {
        var cancelled = await _adminBookingService.CancelBookingAsync(bookingId);

        if (!cancelled)
        {
            return NotFound(new
            {
                success = false,
                message = "Booking not found or already cancelled"
            });
        }

        return Ok(new
        {
            success = true,
            message = "Booking cancelled successfully"
        });
    }
}
