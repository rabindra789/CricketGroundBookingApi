using CricketGroundBookingApi.Data;
using CricketGroundBookingApi.DTOs.Auth;
using CricketGroundBookingApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CricketGroundBookingApi.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileResponse> GetProfileAsync(long userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        return new UserProfileResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role,
            IsVerified = user.IsVerified,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public async Task<UserProfileResponse> UpdateProfileAsync(
        long userId,
        UpdateProfileRequest request
    )
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        var phoneExists = await _context.Users
            .AnyAsync(u => u.Phone == request.Phone && u.Id != userId);

        if (phoneExists)
        {
            throw new Exception("Phone already in use");
        }

        user.FullName = request.FullName;
        user.Phone = request.Phone;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new UserProfileResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role,
            IsVerified = user.IsVerified,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public async Task<bool> ChangePasswordAsync(
        long userId,
        ChangePasswordRequest request
    )
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var isValidPassword = BCrypt.Net.BCrypt.Verify(
            request.CurrentPassword,
            user.PasswordHash
        );

        if (!isValidPassword)
        {
            return false;
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ForgotPasswordAsync(string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return false;
        }

        var resetToken = GenerateResetToken();

        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await SendPasswordResetEmailAsync(email, resetToken, user.FullName);

        return true;
    }

    public async Task<bool> ResetPasswordAsync(
        string email,
        string token,
        string newPassword
    )
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    private string GenerateResetToken()
    {
        return Guid.NewGuid().ToString("N").Substring(0, 32);
    }

    private async Task SendPasswordResetEmailAsync(string email, string token, string userName)
    {
        await Task.Run(() =>
        {
            Console.WriteLine($"[EMAIL] Password reset for {userName} ({email})");
            Console.WriteLine($"[EMAIL] Reset Token: {token}");
            Console.WriteLine($"[EMAIL] Link: https://yourfrontend.com/reset-password?token={token}&email={email}");
        });
    }
}
