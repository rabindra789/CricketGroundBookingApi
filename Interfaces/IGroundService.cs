using CricketGroundBookingApi.DTOs.Grounds;
using CricketGroundBookingApi.Entities;

namespace CricketGroundBookingApi.Interfaces;

public interface IGroundService
{
    Task<Ground> CreateAsync(CreateGroundRequest request);
    Task<List<Ground>> GetAllAsync();
    Task<Ground?> GetByIdAsync(long id);
    Task<Ground?> UpdateAsync(long id, UpdateGroundRequest request);
    Task<bool> DeleteAsync(long id);
}