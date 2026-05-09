using CricketGroundBookingApi.Entities;

namespace CricketGroundBookingApi.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}