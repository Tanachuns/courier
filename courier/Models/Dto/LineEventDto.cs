namespace courier.Models.Dto;

public class LineEventDto
{
    
    public string? Type { get; set; }
    public LineMessageDto Message { get; set; }
    public long?  TimeStamp  { get; set; }
    public LineSourceDto Source { get; set; }
    public string? replytoken { get; set; }
    public string? Mode { get; set; }
    public string? webhookeventid { get; set; }
    public LineDeliveryContextDto DeliveryContext { get; set; }
}