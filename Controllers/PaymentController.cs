using CricketGroundBookingApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CricketGroundBookingApi.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("pay/{bookingId}")]
    public async Task<IActionResult> Pay(long bookingId)
    {
        try
        {
            var result = await _paymentService.PayAsync(bookingId);

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
}