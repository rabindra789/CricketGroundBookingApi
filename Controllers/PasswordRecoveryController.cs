using CricketGroundBookingApi.DTOs.Auth;
using CricketGroundBookingApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CricketGroundBookingApi.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class PasswordRecoveryController : ControllerBase
{
    private readonly IUserService _userService;

    public PasswordRecoveryController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Initiates forgot password flow by sending reset email.
    /// </summary>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.ForgotPasswordAsync(request.Email);

        if (!result)
        {
            return NotFound(new
            {
                success = false,
                message = "Email not found"
            });
        }

        return Ok(new
        {
            success = true,
            message = "Password reset email sent successfully",
            data = new
            {
                email = request.Email
            }
        });
    }

    /// <summary>
    /// Resets user password using reset token.
    /// </summary>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromQuery] string email, ResetPasswordRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(new
            {
                success = false,
                message = "Email parameter is required"
            });
        }

        var result = await _userService.ResetPasswordAsync(email, request.Token, request.NewPassword);

        if (!result)
        {
            return BadRequest(new
            {
                success = false,
                message = "Invalid reset token or email"
            });
        }

        return Ok(new
        {
            success = true,
            message = "Password reset successfully"
        });
    }
}
