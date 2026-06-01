using CricketGroundBookingApi.DTOs.Bookings;

namespace CricketGroundBookingApi.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateBookingAsync(
        long userId,
        CreateBookingRequest request
    );

    Task<List<BookingResponse>> GetUserBookingsAsync(
        long userId
    );

    Task<BookingResponse?> GetBookingByIdAsync(
        long bookingId,
        long userId
    );

    Task<BookingInvoiceResponse?> GetBookingInvoiceByIdAsync(
        long bookingId,
        long? userId = null
    );

    Task<bool> CancelBookingAsync(
        long bookingId,
        long userId
    );

    Task<BookingAvailabilityResponse> GetAvailabilityAsync(
        long groundId,
        DateOnly date
    );
}