namespace CricketGroundBookingApi.Entities;

public class Payment
{
    public long Id { get; set; }

    public long BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    public string PaymentReference { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Status { get; set; } = "Pending"; // Pending / Paid / Failed

    public DateTime? PaidAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}