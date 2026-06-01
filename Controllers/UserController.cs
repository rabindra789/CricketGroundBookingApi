using System.Security.Claims;
using CricketGroundBookingApi.DTOs.Auth;
using CricketGroundBookingApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CricketGroundBookingApi.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCCurrentUser()
    {
        return Ok(new
        {
            success = true,
            message = "Authorized access successful",
            user = new
            {
                Id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                Name = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value,
                Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
                Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
            }
        });
    }

    /// <summary>
    /// Returns the authenticated user's profile.
    /// </summary>
    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        long userId = long.Parse(userIdClaim.Value);

        try
        {
            var profile = await _userService.GetProfileAsync(userId);

            return Ok(new
            {
                success = true,
                data = profile
            });
        }
        catch (Exception ex)
        {
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

    /// <summary>
    /// Updates the authenticated user's profile.
    /// </summary>
    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        long userId = long.Parse(userIdClaim.Value);

        try
        {
            var profile = await _userService.UpdateProfileAsync(userId, request);

            return Ok(new
            {
                success = true,
                message = "Profile updated successfully",
                data = profile
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

    /// <summary>
    /// Changes the authenticated user's password.
    /// </summary>
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        long userId = long.Parse(userIdClaim.Value);

        var changed = await _userService.ChangePasswordAsync(userId, request);

        if (!changed)
        {
            return BadRequest(new
            {
                success = false,
                message = "Current password is incorrect"
            });
        }

        return Ok(new
        {
            success = true,
            message = "Password changed successfully"
        });
    }
}
