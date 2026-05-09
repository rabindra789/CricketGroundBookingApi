namespace CricketGroundBookingApi.DTOs.Grounds;

public class CreateGroundRequest
{
    public string Name {get; set;} = string.Empty;
    public string Description {get; set;} = string.Empty;
    public string Address {get; set;} = string.Empty;
    public decimal Latitude {get; set;}
    public decimal Longitude {get; set;}
}