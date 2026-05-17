namespace CricketGroundBookingApi.Entities;

public class Feedback
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public User User { get; set; } = null!;
    public long GroundId { get; set; }
    public Ground Ground { get; set; } = null!;
    public long BookingId { get; set; }
    public Booking Booking { get; set; } = null!;
    public int Rating { get; set; } // 1-5
    public string Comment { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}