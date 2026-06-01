using System.Text.Json.Serialization;

namespace CricketGroundBookingApi.DTOs.Admin;

public class RevenueByAddonResponse
{
    [JsonPropertyName("addonId")]
    public long AddonId { get; set; }

    [JsonPropertyName("addonName")]
    public string AddonName { get; set; } = string.Empty;

    [JsonPropertyName("totalRevenue")]
    public decimal TotalRevenue { get; set; }

    [JsonPropertyName("timesBooked")]
    public int TimesBooked { get; set; }

    [JsonPropertyName("percentage")]
    public decimal Percentage { get; set; }

    [JsonPropertyName("averagePrice")]
    public decimal AveragePrice { get; set; }
}
