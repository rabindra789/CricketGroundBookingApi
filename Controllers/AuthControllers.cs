using CricketGroundBookingApi.DTOs.Auth;
using CricketGroundBookingApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CricketGroundBookingApi.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);

        return Ok(new
        {
            success = true,
            message = "registration successful",
            data = response
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        return Ok(new
        {
            success = true,
            message = "Login successful",
            data = response
        });
    }
}