using CrispTv.Dtos;
using CrispTv.Interfaces;
using PayStack.Net;

namespace CrispTv.Services;

public class PayStackService : IPayStackService
{
    private readonly IConfiguration _configuration;
    private readonly string payStackTestKey;
    private PayStackApi PayStack { get; set; }
    public PayStackService(IConfiguration configuration)
    {
        _configuration = configuration;
        payStackTestKey = _configuration["PayStack:TestKey"];
        PayStack = new PayStackApi(payStackTestKey);
    }

    public TransactionInitializeResponse InitalizePayment(PaymentRequest request)
    {

        TransactionInitializeRequest createRequest = new()
        {
            AmountInKobo = request.Amount * 100,
            Email = request.EmailAddress,
            Currency = "NGN",
            Reference = Generate().ToString(),
            CallbackUrl = "https://localhost:7242/api/PayStack/verify-payment"

        };
        TransactionInitializeResponse response = PayStack.Transactions.Initialize(createRequest);
        if (response.Status)
        {
            return response;
        }
        throw new NotImplementedException("the payment was unable to go through");
    }

    public TransactionVerifyResponse VerifyPayment(string reference)
    {

        TransactionVerifyResponse response = PayStack.Transactions.Verify(reference);
        if (response.Data.Status == "success")
        {
            return response;
        }
        throw new NotImplementedException("Was not able to complete this request");
    }

    private static int Generate()
    {
        Random rand = new Random((int)DateTime.Now.Ticks);
        return rand.Next(100000000, 999999999);
    }
}
