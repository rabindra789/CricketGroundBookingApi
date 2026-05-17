namespace CricketGroundBookingApi.DTOs.Admin;

public class AdminDashboardResponse
{
    public int TotalBookings { get; set; }
    public int ConfirmedBookings { get; set; }
    public int CancelledBookings { get; set; }
    public int PendingPayments { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TodayBookings { get; set; }
    public int TotalUsers { get; set; }
    public int TotalGrounds { get; set; }
}