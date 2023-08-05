using CrispTv.Dtos;
using RestSharp;

namespace CrispTv.Interfaces;

public interface IFlutterWaveService
{
    ResultResponse InitiatePayment(FlutterWavePaymentDto paymentDto);
    ResultResponse PaymentWebhookAsync(decimal amount, string tx_ref, int transactionId);

    Task<BankResponse> BankAccount();

    /*   string Initiate();*/
}
