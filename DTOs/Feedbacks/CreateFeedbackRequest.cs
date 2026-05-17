namespace CricketGroundBookingApi.DTOs.Feedbacks;

public class CreateFeedbackRequest
{
    public long BookingId {get; set;}
    public int Rating {get; set;}
    public string Comment {get; set;} = string.Empty;
}