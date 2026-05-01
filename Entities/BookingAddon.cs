namespace CricketGroundBookingApi.Entities;

public class BookingAddon
{
    public long Id { get; set; }

    public long BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    public long AddonId { get; set; }
    public Addon Addon { get; set; } = null!;

    public string ItemName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal TotalPrice { get; set; }

    public bool IsComplimentary { get; set; } = false;
}