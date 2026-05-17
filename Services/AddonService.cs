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
            Price = request.Price,
            IsComplimentary = request.IsComplimentary,
            IsActive = true
        };

        _context.Addons.Add(addon);

        await _context.SaveChangesAsync();

        return MapToResponse(addon);
    }

    public async Task<List<AddonResponse>> GetAllAsync()
    {
        var addons = await _context.Addons
            .Where(a => a.IsActive)
            .OrderBy(a => a.Name)
            .ToListAsync();

        return addons.Select(MapToResponse).ToList();
    }
}