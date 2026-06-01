using System.Text.Json.Serialization;

namespace CricketGroundBookingApi.DTOs.Auth;

public class ForgotPasswordResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
