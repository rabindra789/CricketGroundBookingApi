namespace CricketGroundBookingApi.Entities;

public class Addon
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty; // Breakfast, Snacks, Lunch
    public string AddonType { get; set; } = string.Empty; // Food / Package
    public string SportType { get; set; } = "Multi-Sport";

    public decimal Price { get; set; }

    public bool IsComplimentary { get; set; } = false;
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<BookingAddon> BookingAddons { get; set; } = new List<BookingAddon>();
}
