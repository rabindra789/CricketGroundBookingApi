using CricketGroundBookingApi.Data;
using CricketGroundBookingApi.DTOs.Addons;
using CricketGroundBookingApi.Entities;
using CricketGroundBookingApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CricketGroundBookingApi.Services;

public class AddonService : IAddonService
{
    private readonly ApplicationDbContext _context;

    public AddonService(ApplicationDbContext context)
    {
        _context = context;
    }

    private static AddonResponse MapToResponse(Addon addon)
    {
        return new AddonResponse
        {
            Id = addon.Id,
            Name = addon.Name,
            AddonType = addon.AddonType,
            Description = addon.AddonType,
            SportType = addon.SportType,
            Price = addon.Price,
            IsComplimentary = addon.IsComplimentary,
            IsActive = addon.IsActive
        };
    }

    public async Task<AddonResponse> CreateAsync(
        CreateAddonRequest request
    )
    {
        var addon = new Addon
        {
            Name = request.Name,
            AddonType = request.EffectiveDescription,
            SportType = NormalizeSportType(request.SportType),
            Price = request.Price,
            IsComplimentary = request.IsComplimentary,
            IsActive = true
        };

        _context.Addons.Add(addon);

        await _context.SaveChangesAsync();

        return MapToResponse(addon);
    }

    public async Task<List<AddonResponse>> GetAllAsync(string? sportType = null)
    {
        var normalizedSportType = NormalizeSportType(sportType);

        var query = _context.Addons
            .Where(a => a.IsActive);

        if (!string.IsNullOrWhiteSpace(sportType))
        {
            query = query.Where(a =>
                a.SportType == normalizedSportType ||
                a.SportType == "Multi-Sport"
            );
        }

        var addons = await query
            .OrderBy(a => a.Name)
            .ToListAsync();

        return addons.Select(MapToResponse).ToList();
    }

    public async Task<AddonResponse?> UpdateAsync(
        long id,
        CreateAddonRequest request
    )
    {
        var addon = await _context.Addons
            .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);

        if (addon is null)
            return null;

        addon.Name = request.Name;
        addon.AddonType = request.EffectiveDescription;
        addon.SportType = NormalizeSportType(request.SportType);
        addon.Price = request.Price;
        addon.IsComplimentary = request.IsComplimentary;
        addon.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(addon);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var addon = await _context.Addons
            .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);

        if (addon is null)
            return false;

        addon.IsActive = false;
        addon.UpdatedAt = DateTime.UtcNow;

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
