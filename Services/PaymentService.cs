using CricketGroundBookingApi.Data;
using CricketGroundBookingApi.DTOs.Payments;
using CricketGroundBookingApi.Entities;
using CricketGroundBookingApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CricketGroundBookingApi.Services;

public class PaymentService : IPaymentService
{
    private readonly ApplicationDbContext _context;

    public PaymentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentResponse> PayAsync(long bookingId)
    {
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking == null)
        {
            throw new Exception("Booking not found");
        }

        if (booking.PaymentStatus == "Paid")
        {
            throw new Exception("Booking already paid");
        }

        var paymentReference = Guid.NewGuid().ToString();

        var payment = new Payment
        {
            BookingId = booking.Id,
            PaymentReference = paymentReference,
            Amount = booking.TotalAmount,
            Status = "Paid",
            PaidAt = DateTime.UtcNow
        };

        booking.PaymentStatus = "Paid";
        booking.Status = "Confirmed";
        booking.UpdatedAt = DateTime.UtcNow;

        _context.Payments.Add(payment);

        await _context.SaveChangesAsync();

        return new PaymentResponse
        {
            BookingId = booking.Id,
            TransactionId = paymentReference,
            Amount = payment.Amount,
            PaymentStatus = payment.Status,
            PaidAt = payment.PaidAt ?? DateTime.UtcNow
        };
    }
}