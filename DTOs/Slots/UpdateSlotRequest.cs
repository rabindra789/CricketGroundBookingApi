using System.Text.Json.Serialization;

namespace CricketGroundBookingApi.DTOs.Slots;

public class UpdateSlotRequest
{
    public long GroundId { get; set; }
    public string SlotType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan? Start { get; set; }
    public TimeSpan EndTime { get; set; }
    public TimeSpan? End { get; set; }
    public decimal BasePrice { get; set; }
    public decimal? Price { get; set; }
    public bool? IsActive { get; set; }

    [JsonIgnore]
    public string EffectiveSlotType =>
        string.IsNullOrWhiteSpace(Name)
            ? SlotType
            : Name;

    [JsonIgnore]
    public TimeSpan EffectiveStartTime => Start ?? StartTime;

    [JsonIgnore]
    public TimeSpan EffectiveEndTime => End ?? EndTime;

    [JsonIgnore]
    public decimal EffectiveBasePrice => Price ?? BasePrice;
}
