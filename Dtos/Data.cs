namespace CrispTv.Dtos;

public class Data
{
    public string? response_message { get; set; }
    public string? flw_ref { get; set; }
    public string? order_ref { get; set; }
    public string? account_number { get; set; }
    public string? account_status { get; set; }
    public string? bank_name { get; set; }
    public DateTime created_at { get; set; }
    public DateTime expiry_date { get; set; }
    public decimal amount { get; set; }
    public string? note { get; set; }
}
