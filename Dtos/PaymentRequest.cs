using System.ComponentModel.DataAnnotations;

namespace CrispTv.Dtos;

public class PaymentRequest
{
    public int Amount { get; set; }
    [EmailAddress]
    public string EmailAddress { get; set; }
}
