using CricketGroundBookingApi.DTOs.Auth;

namespace CricketGroundBookingApi.Interfaces;

public interface IUserService
{
    Task<UserProfileResponse> GetProfileAsync(long userId);

    Task<UserProfileResponse> UpdateProfileAsync(
        long userId,
        UpdateProfileRequest request
    );

    Task<bool> ChangePasswordAsync(
        long userId,
        ChangePasswordRequest request
    );

    Task<bool> ForgotPasswordAsync(string email);

    Task<bool> ResetPasswordAsync(
        string email,
        string token,
        string newPassword
    );
}
