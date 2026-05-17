namespace CricketGroundBookingApi.DTOs.Feedbacks;

public class FeedbackResponse
{
    public long Id { get; set; }
    public long GroundId { get; set; }
    public string GroundName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}