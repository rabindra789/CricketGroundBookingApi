using CricketGroundBookingApi.DTOs.Admin;

namespace CricketGroundBookingApi.Interfaces;

public interface IAdminService
{
    Task<AdminDashboardResponse> GetDashboardAsync();
}