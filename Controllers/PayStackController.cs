using CrispTv.Dtos;
using CrispTv.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CrispTv.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayStackController : ControllerBase
{
    private readonly IPayStackService _payStackService;

    public PayStackController(IPayStackService payStackService)
	{
       _payStackService = payStackService;
    }

    [HttpPost("make-payment")]
    [SwaggerOperation(Summary = "Make Payment", Description = "Make Payment with Paystack api")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> MakePayment(PaymentRequest request)
    {
        var result = await _payStackService.InitalizePayment(request);
        return Ok(result);
    }

    [HttpPut("verify-payment")]
    [SwaggerOperation(Summary = "Verify Payment by id", Description = "Requires authorization")]
    [SwaggerResponse(StatusCodes.Status200OK, "Return a Transaction Verify Response")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
    public async Task<IActionResult> VerifyPayment(string reference)
    {
        var result = await _payStackService.VerifyPayment(reference);
        SuccessResponse response = new()
        {
            IsSuccessful = result.Status,
            Message = result.Message
        };
        return Ok(response);
    }
}
