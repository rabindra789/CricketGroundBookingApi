namespace CricketGroundBookingApi.DTOs.Grounds;

public class CreateGroundRequest
{
    public string Name {get; set;} = string.Empty;
    public string SportType {get; set;} = "Multi-Sport";
    public string Description {get; set;} = string.Empty;
    public string Address {get; set;} = string.Empty;
    public decimal Latitude {get; set;}
    public decimal Longitude {get; set;}
}
