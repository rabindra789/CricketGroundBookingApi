using CricketGroundBookingApi.Data;
using CricketGroundBookingApi.DTOs.Admin;
using CricketGroundBookingApi.Entities;
using CricketGroundBookingApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CricketGroundBookingApi.Services;

public class AdminRevenueService : IAdminRevenueService
{
    private readonly ApplicationDbContext _context;

    public AdminRevenueService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RevenueSummaryResponse> GetRevenueSummaryAsync(
        DateOnly? startDate,
        DateOnly? endDate,
        string? paymentStatus
    )
    {
        var query = _context.Bookings
            .AsNoTracking()
            .Where(b => b.Status != "Cancelled");

        if (startDate.HasValue)
        {
            query = query.Where(b => b.BookingDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(b => b.BookingDate <= endDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(paymentStatus))
        {
            query = query.Where(b => b.PaymentStatus == paymentStatus.Trim());
        }

        var paidQuery = query.Where(b => b.PaymentStatus == "Paid");

        var totalRevenue = await paidQuery
            .SumAsync(b => (decimal?)b.TotalAmount) ?? 0m;

        var completedPayments = await paidQuery.CountAsync();

        var pendingPayments = await query
            .CountAsync(b => b.PaymentStatus != "Paid");

        var totalBookings = await query.CountAsync();

        var averageBookingValue = completedPayments > 0
            ? Math.Round(totalRevenue / completedPayments, 2)
            : 0m;

        return new RevenueSummaryResponse
        {
            TotalRevenue = totalRevenue,
            TotalBookings = totalBookings,
            CompletedPayments = completedPayments,
            PendingPayments = pendingPayments,
            AverageBookingValue = averageBookingValue,
            Period = new RevenuePeriodResponse
            {
                StartDate = startDate,
                EndDate = endDate
            }
        };
    }

    public async Task<List<RevenueBySlotTypeResponse>> GetRevenueBySlotTypeAsync(
        DateOnly? startDate,
        DateOnly? endDate
    )
    {
        var query = _context.Bookings
            .AsNoTracking()
            .Where(b => b.Status != "Cancelled" && b.PaymentStatus == "Paid");

        if (startDate.HasValue)
        {
            query = query.Where(b => b.BookingDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(b => b.BookingDate <= endDate.Value);
        }

        var grouped = await query
            .GroupBy(b => b.SlotType)
            .Select(group => new
            {
                SlotType = group.Key,
                TotalRevenue = group.Sum(b => b.TotalAmount),
                BookingCount = group.Count()
            })
            .ToListAsync();

        var totalRevenue = grouped.Sum(g => g.TotalRevenue);

        var result = grouped
            .Select(g => new RevenueBySlotTypeResponse
            {
                SlotType = g.SlotType,
                TotalRevenue = g.TotalRevenue,
                BookingCount = g.BookingCount,
                Percentage = totalRevenue > 0
                    ? Math.Round(g.TotalRevenue / totalRevenue * 100m, 2)
                    : 0m
            })
            .OrderByDescending(r => r.TotalRevenue)
            .ToList();

        var expectedTypes = new[] { "Day", "Night", "FullDay" };

        foreach (var type in expectedTypes)
        {
            if (!result.Any(r => r.SlotType == type))
            {
                result.Add(new RevenueBySlotTypeResponse
                {
                    SlotType = type,
                    TotalRevenue = 0m,
                    BookingCount = 0,
                    Percentage = 0m
                });
            }
        }

        return result.OrderByDescending(r => r.TotalRevenue).ToList();
    }

    public async Task<List<RevenueByGroundResponse>> GetRevenueByGroundAsync(
        DateOnly? startDate,
        DateOnly? endDate,
        int limit
    )
    {
        var query = _context.Bookings
            .AsNoTracking()
            .Where(b => b.Status != "Cancelled" && b.PaymentStatus == "Paid");

        if (startDate.HasValue)
        {
            query = query.Where(b => b.BookingDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(b => b.BookingDate <= endDate.Value);
        }

        var grouped = await query
            .GroupBy(b => new { b.GroundId, b.Ground!.Name })
            .Select(group => new
            {
                group.Key.GroundId,
                GroundName = group.Key.Name,
                TotalRevenue = group.Sum(b => b.TotalAmount),
                BookingCount = group.Count()
            })
            .OrderByDescending(g => g.TotalRevenue)
            .Take(limit)
            .ToListAsync();

        var totalRevenue = grouped.Sum(g => g.TotalRevenue);

        return grouped
            .Select(g => new RevenueByGroundResponse
            {
                GroundId = g.GroundId,
                GroundName = g.GroundName,
                TotalRevenue = g.TotalRevenue,
                BookingCount = g.BookingCount,
                Percentage = totalRevenue > 0
                    ? Math.Round(g.TotalRevenue / totalRevenue * 100m, 2)
                    : 0m,
                AveragePerBooking = g.BookingCount > 0
                    ? Math.Round(g.TotalRevenue / g.BookingCount, 2)
                    : 0m
            })
            .ToList();
    }

    public async Task<List<RevenueByAddonResponse>> GetRevenueByAddonAsync(
        DateOnly? startDate,
        DateOnly? endDate
    )
    {
        var query = _context.BookingAddons
            .AsNoTracking()
            .Where(ba => ba.Booking.Status != "Cancelled" && ba.Booking.PaymentStatus == "Paid");

        if (startDate.HasValue)
        {
            query = query.Where(ba => ba.Booking.BookingDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(ba => ba.Booking.BookingDate <= endDate.Value);
        }

        var grouped = await query
            .GroupBy(ba => new { ba.AddonId, ba.Addon!.Name })
            .Select(group => new
            {
                group.Key.AddonId,
                AddonName = group.Key.Name,
                TotalRevenue = group.Sum(ba => ba.TotalPrice),
                TimesBooked = group.Count()
            })
            .OrderByDescending(g => g.TotalRevenue)
            .ToListAsync();

        var totalRevenue = grouped.Sum(g => g.TotalRevenue);

        return grouped
            .Select(g => new RevenueByAddonResponse
            {
                AddonId = g.AddonId,
                AddonName = g.AddonName,
                TotalRevenue = g.TotalRevenue,
                TimesBooked = g.TimesBooked,
                Percentage = totalRevenue > 0
                    ? Math.Round(g.TotalRevenue / totalRevenue * 100m, 2)
                    : 0m,
                AveragePrice = g.TimesBooked > 0
                    ? Math.Round(g.TotalRevenue / g.TimesBooked, 2)
                    : 0m
            })
            .ToList();
    }
}
