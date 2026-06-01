using System.Text.Json.Serialization;

namespace CricketGroundBookingApi.DTOs.Bookings;

public class BookingInvoiceAddonResponse
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("unitPrice")]
    public decimal UnitPrice { get; set; }

    [JsonPropertyName("totalPrice")]
    public decimal TotalPrice { get; set; }

    [JsonPropertyName("isComplimentary")]
    public bool IsComplimentary { get; set; }
}
