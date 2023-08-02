using CrispTv.Dtos;
using PayStack.Net;

namespace CrispTv.Interfaces;

public interface IPayStackService
{
    Task<TransactionInitializeResponse> InitalizePayment(PaymentRequest request);
    Task<TransactionVerifyResponse> VerifyPayment(string reference);
}
