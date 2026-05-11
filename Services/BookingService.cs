using CricketGroundBookingApi.Data;
using CricketGroundBookingApi.DTOs.Bookings;
using CricketGroundBookingApi.Entities;
using CricketGroundBookingApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CricketGroundBookingApi.Services;

public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _context;

    public BookingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BookingResponse> CreateBookingAsync(
        long userId,
        CreateBookingRequest request
    )
    {
        var slot = await _context.Slots
            .FirstOrDefaultAsync(s => s.Id == request.SlotId);

        if (slot == null)
        {
            throw new Exception("Slot not found");
        }

        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        var currentTime = TimeOnly.FromDateTime(DateTime.Now);

        if (request.BookingDate < currentDate)
        {
            throw new Exception("Cannot book past dates");
        }

        if (request.BookingDate == currentDate)
        {
            var slotStartTime = TimeOnly.FromTimeSpan(slot.StartTime);

            if (currentTime >= slotStartTime)
            {
                throw new Exception(
                    "This slot has already started or expired"
                );
            }
        }

        var ground = await _context.Grounds
            .FirstOrDefaultAsync(g => g.Id == request.GroundId);

        if (ground == null)
        {
            throw new Exception("Ground not found");
        }

        // Prevent conflicting bookings
        var existingBookings = await _context.Bookings
            .Where(b =>
                b.GroundId == request.GroundId &&
                b.BookingDate == request.BookingDate &&
                b.Status != "Cancelled"
            )
            .ToListAsync();

        bool conflict = false;

        foreach (var existingBooking in existingBookings)
        {
            // FullDay conflicts with everything
            if (request.SlotType == "FullDay" ||
                existingBooking.SlotType == "FullDay")
            {
                conflict = true;
                break;
            }

            // Same slot type conflict
            if (existingBooking.SlotType == request.SlotType)
            {
                conflict = true;
                break;
            }
        }

        if (conflict)
        {
            throw new Exception("Selected slot is already booked");
        }

        decimal baseAmount = slot.BasePrice;
        decimal addonAmount = 0;

        var booking = new Booking
        {
            UserId = userId,
            GroundId = request.GroundId,
            SlotId = request.SlotId,
            BookingDate = request.BookingDate,
            SlotType = request.SlotType,
            BaseAmount = baseAmount,
            AddonAmount = addonAmount,
            TotalAmount = baseAmount + addonAmount,
            Status = "PendingPayment",
            PaymentStatus = "Pending"
        };

        _context.Bookings.Add(booking);

        await _context.SaveChangesAsync();

        return new BookingResponse
        {
            Id = booking.Id,
            GroundId = booking.GroundId,
            GroundName = ground.Name,
            SlotId = booking.SlotId,
            SlotType = booking.SlotType,
            BookingDate = booking.BookingDate,
            BaseAmount = booking.BaseAmount,
            AddonAmount = booking.AddonAmount,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            PaymentStatus = booking.PaymentStatus,
            CreatedAt = booking.CreatedAt
        };
    }

    public async Task<List<BookingResponse>> GetUserBookingsAsync(
        long userId
    )
    {
        return await _context.Bookings
            .Where(b => b.UserId == userId)
            .Include(b => b.Ground)
            .Select(b => new BookingResponse
            {
                Id = b.Id,
                GroundId = b.GroundId,
                GroundName = b.Ground.Name,
                SlotId = b.SlotId,
                SlotType = b.SlotType,
                BookingDate = b.BookingDate,
                BaseAmount = b.BaseAmount,
                AddonAmount = b.AddonAmount,
                TotalAmount = b.TotalAmount,
                Status = b.Status,
                PaymentStatus = b.PaymentStatus,
                CreatedAt = b.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<BookingResponse?> GetBookingByIdAsync(
        long bookingId,
        long userId
    )
    {
        var booking = await _context.Bookings
            .Include(b => b.Ground)
            .FirstOrDefaultAsync(b =>
                b.Id == bookingId &&
                b.UserId == userId
            );

        if (booking == null)
        {
            return null;
        }

        return new BookingResponse
        {
            Id = booking.Id,
            GroundId = booking.GroundId,
            GroundName = booking.Ground.Name,
            SlotId = booking.SlotId,
            SlotType = booking.SlotType,
            BookingDate = booking.BookingDate,
            BaseAmount = booking.BaseAmount,
            AddonAmount = booking.AddonAmount,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            PaymentStatus = booking.PaymentStatus,
            CreatedAt = booking.CreatedAt
        };
    }

    public async Task<bool> CancelBookingAsync(
        long bookingId,
        long userId
    )
    {
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b =>
                b.Id == bookingId &&
                b.UserId == userId
            );

        if (booking == null)
        {
            return false;
        }

        booking.Status = "Cancelled";
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}