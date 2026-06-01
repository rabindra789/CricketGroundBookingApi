using CricketGroundBookingApi.DTOs.Admin;

namespace CricketGroundBookingApi.Interfaces;

public interface IAdminRevenueService
{
    Task<RevenueSummaryResponse> GetRevenueSummaryAsync(
        DateOnly? startDate,
        DateOnly? endDate,
        string? paymentStatus
    );

    Task<List<RevenueBySlotTypeResponse>> GetRevenueBySlotTypeAsync(
        DateOnly? startDate,
        DateOnly? endDate
    );

    Task<List<RevenueByGroundResponse>> GetRevenueByGroundAsync(
        DateOnly? startDate,
        DateOnly? endDate,
        int limit
    );

    Task<List<RevenueByAddonResponse>> GetRevenueByAddonAsync(
        DateOnly? startDate,
        DateOnly? endDate
    );
}
