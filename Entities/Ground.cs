namespace CricketGroundBookingApi.Entities;

public class Ground
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Slot> Slots { get; set; } = new List<Slot>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public ICollection<Feedback> Feedbacks { get; set; }
    = new List<Feedback>();
}