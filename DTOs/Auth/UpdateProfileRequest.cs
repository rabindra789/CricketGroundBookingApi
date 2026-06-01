using System.ComponentModel.DataAnnotations;

namespace CricketGroundBookingApi.DTOs.Auth;

public class UpdateProfileRequest
{
    [Required]
    [MinLength(3)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;
}
