using System.Text.Json.Serialization;

namespace CricketGroundBookingApi.DTOs.Admin;

public class RevenueSummaryResponse
{
    [JsonPropertyName("totalRevenue")]
    public decimal TotalRevenue { get; set; }

    [JsonPropertyName("totalBookings")]
    public int TotalBookings { get; set; }

    [JsonPropertyName("completedPayments")]
    public int CompletedPayments { get; set; }

    [JsonPropertyName("pendingPayments")]
    public int PendingPayments { get; set; }

    [JsonPropertyName("averageBookingValue")]
    public decimal AverageBookingValue { get; set; }

    [JsonPropertyName("period")]
    public RevenuePeriodResponse Period { get; set; } = new();
}

public class RevenuePeriodResponse
{
    [JsonPropertyName("startDate")]
    public DateOnly? StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public DateOnly? EndDate { get; set; }
}
