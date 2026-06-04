namespace CricketGroundBookingApi.DTOs.Addons;

public class AddonResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AddonType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SportType { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsComplimentary { get; set; }
    public bool IsActive { get; set; }
}
