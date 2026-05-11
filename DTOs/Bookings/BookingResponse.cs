namespace CricketGroundBookingApi.DTOs.Bookings;

public class BookingResponse
{
    public long Id { get; set; }
    public long GroundId { get; set; }
    public string GroundName { get; set; } = string.Empty;
    public long SlotId { get; set; }
    public string SlotType { get; set; } = string.Empty;
    public DateOnly BookingDate { get; set; }
    public decimal BaseAmount { get; set; }
    public decimal AddonAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}