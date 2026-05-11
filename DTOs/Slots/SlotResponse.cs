namespace CricketGroundBookingApi.DTOs.Slots;

public class SlotResponse
{
    public long Id {get; set;}
    public long GroundId {get; set;}
    public string SlotType {get; set;} = string.Empty;
    public TimeSpan StartTime {get; set;}
    public TimeSpan EndTime {get; set;}
    public decimal BasePrice {get; set;}
    public bool IsActive {get; set;}
}