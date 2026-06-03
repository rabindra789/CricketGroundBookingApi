using CricketGroundBookingApi.DTOs.Addons;

namespace CricketGroundBookingApi.Interfaces;

public interface IAddonService
{
    Task<AddonResponse> CreateAsync(
        CreateAddonRequest request
    );

    Task<List<AddonResponse>> GetAllAsync(string? sportType = null);
}
