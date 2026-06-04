using System.Text.Json.Serialization;

namespace CricketGroundBookingApi.DTOs.Addons;

public class CreateAddonRequest
{
    public string Name {get; set;} = string.Empty;
    public string AddonType {get; set;} = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SportType {get; set;} = "Multi-Sport";
    public decimal Price {get; set;}
    public bool IsComplimentary {get; set;}

    [JsonIgnore]
    public string EffectiveDescription =>
        string.IsNullOrWhiteSpace(Description)
            ? AddonType
            : Description;
}
