using System.Text.Json.Serialization;

namespace CricketGroundBookingApi.DTOs.Admin;

public class AdminBookingResponse
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("groundId")]
    public long GroundId { get; set; }

    [JsonPropertyName("groundName")]
    public string GroundName { get; set; } = string.Empty;

    [JsonPropertyName("userId")]
    public long UserId { get; set; }

    [JsonPropertyName("userName")]
    public string UserName { get; set; } = string.Empty;

    [JsonPropertyName("userPhone")]
    public string UserPhone { get; set; } = string.Empty;

    [JsonPropertyName("bookingDate")]
    public DateOnly BookingDate { get; set; }

    [JsonPropertyName("slotType")]
    public string SlotType { get; set; } = string.Empty;

    [JsonPropertyName("startTime")]
    public TimeSpan StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public TimeSpan EndTime { get; set; }

    [JsonPropertyName("totalAmount")]
    public decimal TotalAmount { get; set; }

    [JsonPropertyName("paymentStatus")]
    public string PaymentStatus { get; set; } = string.Empty;

    [JsonPropertyName("bookingStatus")]
    public string BookingStatus { get; set; } = string.Empty;

    [JsonPropertyName("addons")]
    public List<AdminBookingAddonResponse> Addons { get; set; } = new();

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
}
