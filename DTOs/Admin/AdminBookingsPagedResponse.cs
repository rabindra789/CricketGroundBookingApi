using System.Text.Json.Serialization;

namespace CricketGroundBookingApi.DTOs.Admin;

public class AdminBookingsPagedResponse
{
    [JsonPropertyName("bookings")]
    public List<AdminBookingResponse> Bookings { get; set; } = new();

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("pageNo")]
    public int PageNo { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }
}
