namespace CrispTv.Dtos;

public class ResultResponse
{
    public bool IsSuccessful { get; set; }
    public string Message { get; set; } = null!;
    public string? Data { get; set; }
}
