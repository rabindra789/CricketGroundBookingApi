namespace CricketGroundBookingApi.Entities;

public class Slot
{
    public long Id { get; set; }

    public long GroundId { get; set; }
    public Ground Ground { get; set; } = null!;

    public string SlotType { get; set; } = string.Empty; // Day / Night

    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public decimal BasePrice { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}