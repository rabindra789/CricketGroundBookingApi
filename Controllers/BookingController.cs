using System.Security.Claims;
using CricketGroundBookingApi.DTOs.Bookings;
using CricketGroundBookingApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CricketGroundBookingApi.Controllers;

[ApiController]
[Route("api/v1/Booking")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(
        CreateBookingRequest request
    )
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        long userId = long.Parse(userIdClaim.Value);

        var booking = await _bookingService.CreateBookingAsync(
            userId,
            request
        );

        return Ok(new
        {
            success = true,
            message = "Booking created successfully",
            data = booking
        });
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        long userId = long.Parse(userIdClaim.Value);

        var bookings = await _bookingService
            .GetUserBookingsAsync(userId);

        return Ok(new
        {
            success = true,
            data = bookings
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(long id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        long userId = long.Parse(userIdClaim.Value);

        var booking = await _bookingService
            .GetBookingByIdAsync(id, userId);

        if (booking == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Booking not found"
            });
        }

        return Ok(new
        {
            success = true,
            data = booking
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelBooking(long id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        long userId = long.Parse(userIdClaim.Value);

        var cancelled = await _bookingService
            .CancelBookingAsync(id, userId);

        if (!cancelled)
        {
            return NotFound(new
            {
                success = false,
                message = "Booking not found"
            });
        }

        return Ok(new
        {
            success = true,
            message = "Booking cancelled successfully"
        });
    }
}