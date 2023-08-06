using System.ComponentModel.DataAnnotations;

namespace CrispTv.Dtos;

public class FlutterWavePaymentDto
{
    public decimal amount { get; set; }
    [EmailAddress]
    public string emailAddress { get; set; } = null!;
    public string?  phoneNumber { get; set; } 
    public string? name { get; set; }
 
}
