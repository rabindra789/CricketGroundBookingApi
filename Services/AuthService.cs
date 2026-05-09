using CricketGroundBookingApi.Data;
using CricketGroundBookingApi.DTOs.Auth;
using CricketGroundBookingApi.Entities;
using CricketGroundBookingApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CricketGroundBookingApi.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthService(ApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var emailExists = await _context.Users.AnyAsync(x => x.Email == request.Email);

        if(emailExists)
            throw new Exception("Email already exists");
        
        var phoneExists = await _context.Users.AnyAsync(x => x.Phone == request.Phone);
        
        if (phoneExists)
            throw new Exception("Phone already exists.");

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

        var token = _jwtService.GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
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

        var token = _jwtService.GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        };
    }
}