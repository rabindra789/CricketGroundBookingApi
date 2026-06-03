using CricketGroundBookingApi.Data;
using CricketGroundBookingApi.DTOs.Grounds;
using CricketGroundBookingApi.Entities;
using CricketGroundBookingApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CricketGroundBookingApi.Services;

public class GroundService : IGroundService
{
    private readonly ApplicationDbContext _context;

    public GroundService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Ground> CreateAsync(CreateGroundRequest request)
    {
        var ground = new Ground
        {
            Name = request.Name,
            SportType = NormalizeSportType(request.SportType),
            Description = request.Description,
            Address = request.Address,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };

        _context.Grounds.Add(ground);

        await _context.SaveChangesAsync();

        return ground;
    }

    public async Task<List<Ground>> GetAllAsync()
    {
        return await _context.Grounds
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();
    }

    public async Task<Ground?> GetByIdAsync(long id)
    {
        return await _context.Grounds
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Ground?> UpdateAsync(
        long id,
        UpdateGroundRequest request
    )
    {
        var ground = await _context.Grounds
            .FirstOrDefaultAsync(g => g.Id == id);

        if (ground is null)
            return null;

        ground.Name = request.Name;
        ground.SportType = NormalizeSportType(request.SportType);
        ground.Description = request.Description;
        ground.Address = request.Address;
        ground.Latitude = request.Latitude;
        ground.Longitude = request.Longitude;
        ground.IsActive = request.IsActive;
        ground.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ground;
    }
    public async Task<bool> DeleteAsync(long id)
    {
        var ground = await _context.Grounds
        .FirstOrDefaultAsync(g => g.Id == id);

        if (ground is null)
            return false;

        _context.Grounds.Remove(ground);

        await _context.SaveChangesAsync();

        return true;
    }

    private static string NormalizeSportType(string? sportType)
    {
        return string.IsNullOrWhiteSpace(sportType)
            ? "Multi-Sport"
            : sportType.Trim();
    }
}
