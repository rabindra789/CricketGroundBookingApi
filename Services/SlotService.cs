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

    public async Task<Slot> CreateAsync(CreateSlotRequest request)
    {
        var groundExists = await _context.Grounds.AnyAsync(g => g.Id == request.GroundId);

        if (!groundExists)
            throw new Exception("Ground not found.");

        var slotExists = await _context.Slots.AnyAsync(s =>
            s.GroundId == request.GroundId &&
            s.SlotType.ToLower() == request.SlotType.ToLower()
        );

        if (slotExists)
            throw new Exception($"Slot type '{request.SlotType}' already exists for this ground.");

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

        return slot;
    }

    public async Task<List<Slot>> GetAllAsync()
    {
        return await _context.Slots.Include(s => s.Ground).OrderBy(s => s.GroundId).ToListAsync();
    }

    public async Task<List<Slot>> GetByGroundIdAsync(long groundId)
    {
        return await _context.Slots.Where(s => s.GroundId == groundId).OrderBy(s => s.StartTime).ToListAsync();
    }

    public async Task<Slot?> GetByIdAsync(long id)
    {
        return await _context.Slots.Include(s => s.Ground).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Slot?> UpdateAsync(
        long id,
        UpdateSlotRequest request
    )
    {
        var slot = await _context.Slots.FirstOrDefaultAsync(s => s.Id == id);

        if (slot is null)
            return null;

        slot.SlotType = request.SlotType;
        slot.StartTime = request.StartTime;
        slot.EndTime = request.EndTime;
        slot.BasePrice = request.BasePrice;
        slot.IsActive = request.IsActive;
        slot.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return slot;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var slot = await _context.Slots.FirstOrDefaultAsync(s => s.Id == id);

        if (slot is null)
            return false;

        _context.Slots.Remove(slot);

        await _context.SaveChangesAsync();

        return true;
    }
}