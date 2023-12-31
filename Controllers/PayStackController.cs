﻿using CrispTv.Dtos;
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
    public IActionResult MakePayment(PaymentRequest request)
    {
        var result =  _payStackService.InitalizePayment(request);
        return Ok(result);
    }

    [HttpPut("verify-payment")]
    public IActionResult VerifyPayment(string reference)
    {
        var result =  _payStackService.VerifyPayment(reference);
        SuccessResponse response = new()
        {
            IsSuccessful = result.Status,
            Message = result.Message
        };
        return Ok(response);
    }
}
