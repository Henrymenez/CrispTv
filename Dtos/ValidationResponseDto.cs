namespace CrispTv.Dtos;

public class ValidationResponseDto
{
    public string status { get; set; } = null!;
    public string message { get; set; } = null!;
    public Data data { get; set; } = null!;
}
