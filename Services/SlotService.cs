using CricketGroundBookingApi.Data;
using CricketGroundBookingApi.DTOs.Slots;
using CricketGroundBookingApi.Entities;
using CricketGroundBookingApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CricketGroundBookingApi.Services;

public class SlotService : ISlotService
{
    private readonly ApplicationDbContext _context;

    public SlotService(ApplicationDbContext context)
    {
        _context = context;
    }

    private static SlotResponse MapToResponse(Slot slot)
    {
        return new SlotResponse
        {
            Id = slot.Id,
            GroundId = slot.GroundId,
            SlotType = slot.SlotType,
            StartTime = slot.StartTime,
            EndTime = slot.EndTime,
            BasePrice = slot.BasePrice,
            IsActive = slot.IsActive
        };
    }

    public async Task<SlotResponse> CreateAsync(CreateSlotRequest request)
    {
        var groundExists = await _context.Grounds
            .AnyAsync(g => g.Id == request.GroundId);

        if (!groundExists)
            throw new Exception("Ground not found.");

        var slotExists = await _context.Slots.AnyAsync(s =>
            s.GroundId == request.GroundId &&
            s.SlotType.ToLower() == request.SlotType.ToLower()
        );

        if (slotExists)
            throw new Exception(
                $"Slot type '{request.SlotType}' already exists for this ground."
            );

        var slot = new Slot
        {
            GroundId = request.GroundId,
            SlotType = request.SlotType,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            BasePrice = request.BasePrice
        };

        _context.Slots.Add(slot);

        await _context.SaveChangesAsync();

        return MapToResponse(slot);
    }

    public async Task<List<SlotResponse>> GetAllAsync()
    {
        var slots = await _context.Slots
            .OrderBy(s => s.GroundId)
            .ToListAsync();

        return slots.Select(MapToResponse).ToList();
    }

    public async Task<List<SlotResponse>> GetByGroundIdAsync(long groundId)
    {
        var slots = await _context.Slots
            .Where(s => s.GroundId == groundId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();

        return slots.Select(MapToResponse).ToList();
    }

    public async Task<SlotResponse?> GetByIdAsync(long id)
    {
        var slot = await _context.Slots
            .FirstOrDefaultAsync(s => s.Id == id);

        if (slot == null)
            return null;

        return MapToResponse(slot);
    }

    public async Task<SlotResponse?> UpdateAsync(
        long id,
        UpdateSlotRequest request
    )
    {
        var slot = await _context.Slots
            .FirstOrDefaultAsync(s => s.Id == id);

        if (slot is null)
            return null;

        slot.SlotType = request.SlotType;
        slot.StartTime = request.StartTime;
        slot.EndTime = request.EndTime;
        slot.BasePrice = request.BasePrice;
        slot.IsActive = request.IsActive;
        slot.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(slot);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var slot = await _context.Slots
            .FirstOrDefaultAsync(s => s.Id == id);

        if (slot is null)
            return false;

        _context.Slots.Remove(slot);

        await _context.SaveChangesAsync();

        return true;
    }
}