using CricketGroundBookingApi.DTOs.Payments;

namespace CricketGroundBookingApi.Interfaces;

public interface IPaymentService
{
    Task<PaymentResponse> PayAsync(long BookingId);
}