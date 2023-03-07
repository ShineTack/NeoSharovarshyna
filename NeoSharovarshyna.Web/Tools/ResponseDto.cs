namespace NeoSharovarshyna.Web.Tools;

public class ResponseDto
{
    public bool IsSuccess { get; set; } = true;
    public object? Data { get; set; }
    public string? DisplayMessage { get; set; }
    public List<string>? ErrorMessages { get; set; }
}
