using CricketGroundBookingApi.Data;
using CricketGroundBookingApi.DTOs.Auth;
using CricketGroundBookingApi.Entities;
using CricketGroundBookingApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace CricketGroundBookingApi.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var emailExists = await _context.Users.AnyAsync(x => x.Email == request.Email);
        if(emailExists)
            throw new Exception("Email already exists");

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "User",
            IsVerified = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            Token = string.Empty, //TODO: JWT next step
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.EmailOrPhone || x.Phone == request.EmailOrPhone);

        if(user is null)
            throw new Exception("Invalid credentials.");

        var validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if(!validPassword)
            throw new Exception("Invalid Password.");

        return new AuthResponse
        {
            Token = string.Empty, //TODO: JWT next step
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        };
    }
}