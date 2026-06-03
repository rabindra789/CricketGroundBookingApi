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
            AddonType = request.AddonType,
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

    private static string NormalizeSportType(string? sportType)
    {
        return string.IsNullOrWhiteSpace(sportType)
            ? "Multi-Sport"
            : sportType.Trim();
    }
}
