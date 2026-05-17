using System.ComponentModel.DataAnnotations;

namespace CricketGroundBookingApi.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    [MinLength(3)]
    public string FullName { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}