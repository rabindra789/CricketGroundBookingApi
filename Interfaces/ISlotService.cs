using CricketGroundBookingApi.DTOs.Slots;
using CricketGroundBookingApi.Entities;

namespace CricketGroundBookingApi.Interfaces;

public interface ISlotService
{
    Task<SlotResponse> CreateAsync(CreateSlotRequest request);
    Task<List<SlotResponse>> GetAllAsync();
    Task<List<SlotResponse>> GetByGroundIdAsync(long groundId);
    Task<SlotResponse?> GetByIdAsync(long id);
    Task<SlotResponse?> UpdateAsync(long id, UpdateSlotRequest request);
    Task<bool> DeleteAsync(long id);
}