using CrispTv.Dtos;
using CrispTv.Interfaces;
using Flutterwave.Net;

namespace CrispTv.Services;

public class FlutterWaveService : IFlutterWaveService
{
    private readonly IConfiguration _configuration;
    private readonly string flutterwaveSecretKey;
    private readonly string RedirectUrl;

    private FlutterwaveApi Flutterwave { get; set; }

    public FlutterWaveService(IConfiguration configuration)
    {
        _configuration = configuration;
        flutterwaveSecretKey = _configuration["FlutterWave:SecretKey"];
        RedirectUrl = _configuration["FlutterWave:RedirectUrl"];
        Flutterwave = new FlutterwaveApi(flutterwaveSecretKey);
    }
    public string InitiatePayment(FlutterWavePaymentDto paymentDto)
    {
        var tnx_ref = GenerateRandomString(20);
        InitiatePaymentResponse response = Flutterwave.Payments.InitiatePayment(tnx_ref, paymentDto.amount, RedirectUrl, paymentDto.name, paymentDto.emailAddress,
                                                                 paymentDto.phoneNumber, "Order", "This is coming from test app", "incoming.com");
        if (response.Status == "success")
        {
            string hostedLink = response.Data.Link;

            return hostedLink;
        }
        return response.Message;
    }

    public string PaymentWebhookAsync(decimal amount, string tx_ref, int transactionId)
    {

        VerifyTransactionResponse response = Flutterwave.Transactions.VerifyTransaction(transactionId);
        Transaction transaction = response.Data;

        if (transaction.Status == "successful" && response.Status == "success" && transaction.Amount == amount && transaction.TxRef == tx_ref)
        {
            return "Tarnsaction was successful";
        }

        return "Transaction Failed";
    }

    public static string GenerateRandomString(int length)
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        Random random = new Random();
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}

