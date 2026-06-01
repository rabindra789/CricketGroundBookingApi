using CricketGroundBookingApi.DTOs.Admin;

namespace CricketGroundBookingApi.Interfaces;

public interface IAdminBookingService
{
    Task<AdminBookingsPagedResponse> GetBookingsAsync(
        int pageNo,
        int pageSize,
        string? status,
        long? groundId,
        DateOnly? startDate,
        DateOnly? endDate
    );

    Task<bool> CancelBookingAsync(long bookingId);
}
