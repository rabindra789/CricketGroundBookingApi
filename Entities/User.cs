namespace CricketGroundBookingApi.Entities;

public class User
{
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "User"; // if Admin or User

    public bool IsVerified { get; set; } = false;
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Booking> Bookings {get; set;} = new List<Booking>();
    
    public ICollection<Feedback> Feedbacks { get; set; }
    = new List<Feedback>();
}