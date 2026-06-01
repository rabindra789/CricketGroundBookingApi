using CricketGroundBookingApi.Data;
using CricketGroundBookingApi.DTOs.Admin;
using CricketGroundBookingApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CricketGroundBookingApi.Services;

public class AdminBookingService : IAdminBookingService
{
    private readonly ApplicationDbContext _context;

    public AdminBookingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdminBookingsPagedResponse> GetBookingsAsync(
        int pageNo,
        int pageSize,
        string? status,
        long? groundId,
        DateOnly? startDate,
        DateOnly? endDate
    )
    {
        var query = _context.Bookings
            .AsNoTracking()
            .Include(b => b.User)
            .Include(b => b.Ground)
            .Include(b => b.Slot)
            .Include(b => b.BookingAddons)
                .ThenInclude(ba => ba.Addon)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(b => b.Status == status.Trim());
        }

        if (groundId.HasValue)
        {
            query = query.Where(b => b.GroundId == groundId.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(b => b.BookingDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(b => b.BookingDate <= endDate.Value);
        }

        var totalCount = await query.CountAsync();

        var bookings = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new AdminBookingResponse
            {
                Id = b.Id,
                GroundId = b.GroundId,
                GroundName = b.Ground.Name,
                UserId = b.UserId,
                UserName = b.User.FullName,
                UserPhone = b.User.Phone,
                BookingDate = b.BookingDate,
                SlotType = b.SlotType,
                StartTime = b.Slot.StartTime,
                EndTime = b.Slot.EndTime,
                TotalAmount = b.TotalAmount,
                PaymentStatus = b.PaymentStatus,
                BookingStatus = b.Status,
                CreatedAt = b.CreatedAt,
                Addons = b.BookingAddons
                    .Select(ba => new AdminBookingAddonResponse
                    {
                        Id = ba.Id,
                        Name = ba.Addon.Name,
                        Price = ba.Addon.Price
                    })
                    .ToList()
            })
            .ToListAsync();

        return new AdminBookingsPagedResponse
        {
            Bookings = bookings,
            TotalCount = totalCount,
            PageNo = pageNo,
            PageSize = pageSize
        };
    }

    public async Task<bool> CancelBookingAsync(long bookingId)
    {
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking == null || booking.Status == "Cancelled")
        {
            return false;
        }

        booking.Status = "Cancelled";
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}
