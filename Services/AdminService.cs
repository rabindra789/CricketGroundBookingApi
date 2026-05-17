using CricketGroundBookingApi.Data;
using CricketGroundBookingApi.DTOs.Admin;
using CricketGroundBookingApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CricketGroundBookingApi.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _context;

    public AdminService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdminDashboardResponse> GetDashboardAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        return new AdminDashboardResponse
        {
            TotalBookings = await _context.Bookings.CountAsync(),
            
            ConfirmedBookings = await _context.Bookings
                .CountAsync(b => b.Status == "Confirmed"),

            CancelledBookings = await _context.Bookings
                .CountAsync(b => b.Status == "Cancelled"),

            PendingPayments = await _context.Bookings
                .CountAsync(b => b.PaymentStatus == "Pending"),

            TotalRevenue = await _context.Bookings
                .Where(b => b.PaymentStatus == "Paid")
                .SumAsync(b => (decimal?)b.TotalAmount)
                    ?? 0,

            TodayBookings = await _context.Bookings
                .CountAsync(b => b.BookingDate == today),

            TotalUsers = await _context.Users.CountAsync(),

            TotalGrounds = await _context.Grounds.CountAsync()
        };
    }
}