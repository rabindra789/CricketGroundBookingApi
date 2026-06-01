using System.Text.Json.Serialization;

namespace CricketGroundBookingApi.DTOs.Admin;

public class AdminBookingAddonResponse
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}
