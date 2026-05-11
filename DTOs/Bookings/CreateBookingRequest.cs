using System.ComponentModel.DataAnnotations;

namespace CricketGroundBookingApi.DTOs.Bookings;

public class CreateBookingRequest
{
    [Required]
    public long GroundId { get; set; }
    [Required]
    public long SlotId { get; set; }
    [Required]
    public DateOnly BookingDate { get; set; }
    [Required]
    public string SlotType { get; set; } = string.Empty;
    public List<long> AddonIds { get; set; } = [];
}