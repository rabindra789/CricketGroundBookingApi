namespace CricketGroundBookingApi.DTOs.Payments;

public class PaymentResponse
{
    public long BookingId {get; set;}
    public string TransactionId {get; set;} = string.Empty;
    public decimal Amount {get; set;}
    public string PaymentStatus {get; set;} = string.Empty;
    public DateTime PaidAt {get; set;}
}