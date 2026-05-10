using CricketGroundBookingApi.DTOs.Slots;
using CricketGroundBookingApi.Entities;

namespace CricketGroundBookingApi.Interfaces;

public interface ISlotService
{
    Task<Slot> CreateAsync(CreateSlotRequest request);
    Task<List<Slot>> GetAllAsync();
    Task<List<Slot>> GetByGroundIdAsync(long groundId);
    Task<Slot?> GetByIdAsync(long id);
    Task<Slot?> UpdateAsync(long id, UpdateSlotRequest request);
    Task<bool> DeleteAsync(long id);
}