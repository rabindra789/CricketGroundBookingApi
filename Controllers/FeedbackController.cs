using CricketGroundBookingApi.DTOs.Feedbacks;
using CricketGroundBookingApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CricketGroundBookingApi.Controllers;

[ApiController]
[Route("api/v1/feedback")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateFeedbackRequest request
    )
    {
        try
        {
            var userId = long.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var result = await _feedbackService
                .CreateAsync(userId, request);

            return Ok(new
            {
                success = true,
                data = result
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

    [HttpGet("ground/{groundId}")]
    public async Task<IActionResult> GetByGround(
        long groundId
    )
    {
        var result = await _feedbackService
            .GetByGroundAsync(groundId);

        return Ok(new
        {
            success = true,
            data = result
        });
    }
}