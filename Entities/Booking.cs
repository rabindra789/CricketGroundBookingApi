namespace CricketGroundBookingApi.Entities;

public class Booking
{
    public long Id { get; set; }

    public long UserId { get; set; }
    public User User { get; set; } = null!;

    public long GroundId { get; set; }
    public Ground Ground { get; set; } = null!;

    public long SlotId { get; set; }
    public Slot Slot { get; set; } = null!;

    public DateOnly BookingDate { get; set; }

    public string SlotType { get; set; } = string.Empty; // Day / Night

    public string Status { get; set; } = "PendingPayment"; // PendingPayment / Confirmed / Cancelled

    public decimal BaseAmount { get; set; }
    public decimal AddonAmount { get; set; }
    public decimal TotalAmount { get; set; }

    public string PaymentStatus { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<BookingAddon> BookingAddons { get; set; } = new List<BookingAddon>();
    public Payment? Payment { get; set; }
}