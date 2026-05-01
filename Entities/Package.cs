namespace CricketGroundBookingApi.Entities;

public class Package
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty; // Basic / Advanced
    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}