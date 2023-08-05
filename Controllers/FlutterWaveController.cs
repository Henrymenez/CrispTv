using CrispTv.Dtos;
using CrispTv.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        var result = _flutterWaveService.InitiatePayment(request);
        if (result.IsSuccessful)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost("make-bank-payment")]
    public async Task<IActionResult> MakeBankPayment()
    {
        var result = await _flutterWaveService.BankAccount();
        return Ok(result);
    }

    [HttpGet("verify-payment")]
    public async Task<IActionResult> WebHookPayment(decimal amount, string tx_ref, int transactionId)
    {
        var result = _flutterWaveService.PaymentWebhookAsync(amount, tx_ref, transactionId);
        if (result.IsSuccessful)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }


}
