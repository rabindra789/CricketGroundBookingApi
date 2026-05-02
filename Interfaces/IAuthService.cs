using CricketGroundBookingApi.DTOs.Auth;

namespace CricketGroundBookingApi.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}