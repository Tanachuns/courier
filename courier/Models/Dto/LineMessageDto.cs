using Newtonsoft.Json;

namespace courier.Models.Dto;

public class LineMessageDto
{
    public string? Type { get; set; }
    public string? Id { get; set; }
    public string? Text { get; set; }
    public string? QuoteToken { get; set; }
    public string? MarkAsReadToken { get; set; }
}