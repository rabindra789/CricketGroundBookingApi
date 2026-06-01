using System.Text.Json.Serialization;

namespace CricketGroundBookingApi.DTOs.Admin;

public class RevenueByGroundResponse
{
    [JsonPropertyName("groundId")]
    public long GroundId { get; set; }

    [JsonPropertyName("groundName")]
    public string GroundName { get; set; } = string.Empty;

    [JsonPropertyName("totalRevenue")]
    public decimal TotalRevenue { get; set; }

    [JsonPropertyName("bookingCount")]
    public int BookingCount { get; set; }

    [JsonPropertyName("percentage")]
    public decimal Percentage { get; set; }

    [JsonPropertyName("averagePerBooking")]
    public decimal AveragePerBooking { get; set; }
}
