namespace CricketGroundBookingApi.DTOs.Slots;

public class CreateSlotRequest
{
    public long GroundId { get; set; }
    public string SlotType { get; set; } = string.Empty; //! Day / Night
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public decimal BasePrice { get; set; }
}