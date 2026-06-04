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
            Name = slot.SlotType,
            StartTime = slot.StartTime,
            Start = slot.StartTime,
            EndTime = slot.EndTime,
            End = slot.EndTime,
            BasePrice = slot.BasePrice,
            Price = slot.BasePrice,
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
            s.IsActive &&
            s.SlotType.ToLower() == request.EffectiveSlotType.ToLower()
        );

        if (slotExists)
            throw new Exception(
                $"Slot type '{request.EffectiveSlotType}' already exists for this ground."
            );

        var slot = new Slot
        {
            GroundId = request.GroundId,
            SlotType = request.EffectiveSlotType,
            StartTime = request.EffectiveStartTime,
            EndTime = request.EffectiveEndTime,
            BasePrice = request.EffectiveBasePrice
        };

        _context.Slots.Add(slot);

        await _context.SaveChangesAsync();

        return MapToResponse(slot);
    }

    public async Task<List<SlotResponse>> GetAllAsync()
    {
        var slots = await _context.Slots
            .Where(s => s.IsActive)
            .OrderBy(s => s.GroundId)
            .ToListAsync();

        return slots.Select(MapToResponse).ToList();
    }

    public async Task<List<SlotResponse>> GetByGroundIdAsync(long groundId)
    {
        var slots = await _context.Slots
            .Where(s => s.GroundId == groundId && s.IsActive)
            .OrderBy(s => s.StartTime)
            .ToListAsync();

        return slots.Select(MapToResponse).ToList();
    }

    public async Task<SlotResponse?> GetByIdAsync(long id)
    {
        var slot = await _context.Slots
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

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
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

        if (slot is null)
            return null;

        slot.SlotType = request.EffectiveSlotType;
        slot.StartTime = request.EffectiveStartTime;
        slot.EndTime = request.EffectiveEndTime;
        slot.BasePrice = request.EffectiveBasePrice;
        slot.IsActive = request.IsActive ?? slot.IsActive;
        slot.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(slot);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var slot = await _context.Slots
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

        if (slot is null)
            return false;

        slot.IsActive = false;
        slot.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}
