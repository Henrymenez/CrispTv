using CrispTv.Dtos;
using PayStack.Net;

namespace CrispTv.Interfaces;

public interface IPayStackService
{
    TransactionInitializeResponse InitalizePayment(PaymentRequest request);
    TransactionVerifyResponse VerifyPayment(string reference);
}
