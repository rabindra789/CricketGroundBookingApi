using System.Text.Json.Serialization;

namespace CricketGroundBookingApi.DTOs.Admin;

public class RevenueBySlotTypeResponse
{
    [JsonPropertyName("slotType")]
    public string SlotType { get; set; } = string.Empty;

    [JsonPropertyName("totalRevenue")]
    public decimal TotalRevenue { get; set; }

    [JsonPropertyName("bookingCount")]
    public int BookingCount { get; set; }

    [JsonPropertyName("percentage")]
    public decimal Percentage { get; set; }
}
