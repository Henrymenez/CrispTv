using CrispTv.Dtos;
using RestSharp;

namespace CrispTv.Interfaces;

public interface IFlutterWaveService
{
    string InitiatePayment(FlutterWavePaymentDto paymentDto);
    string PaymentWebhookAsync(decimal amount, string tx_ref, int transactionId);
    Task<bool> VerifyPayment(string transactionId);
}
