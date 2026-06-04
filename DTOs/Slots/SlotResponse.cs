namespace CricketGroundBookingApi.DTOs.Slots;

public class SlotResponse
{
    public long Id {get; set;}
    public long GroundId {get; set;}
    public string SlotType {get; set;} = string.Empty;
    public string Name { get; set; } = string.Empty;
    public TimeSpan StartTime {get; set;}
    public TimeSpan Start { get; set; }
    public TimeSpan EndTime {get; set;}
    public TimeSpan End { get; set; }
    public decimal BasePrice {get; set;}
    public decimal Price { get; set; }
    public bool IsActive {get; set;}
}
