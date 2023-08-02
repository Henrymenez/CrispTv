using CrispTv.Dtos;
using CrispTv.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CrispTv.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlutterWaveController : ControllerBase
{
    private readonly IFlutterWaveService _flutterWaveService;

    public FlutterWaveController(IFlutterWaveService flutterWaveService)
	{
       
        _flutterWaveService = flutterWaveService;
    }


    [HttpPost("make-payment")]
    public async Task<IActionResult> MakePayment(FlutterWavePaymentDto request)
    {
        var result =  _flutterWaveService.InitiatePayment(request);
        return Ok(result);
    }

    [HttpGet("verify-payment")]
    public async Task<IActionResult> WebHookPayment(decimal amount, string tx_ref, int transactionId)
    {
         var result =  _flutterWaveService.PaymentWebhookAsync(amount,tx_ref,transactionId);
        return Ok(result);
    }
}
