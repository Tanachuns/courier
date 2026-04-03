namespace courier.Models.Dto;

public class LineEventDto
{
    /*
      "deliveryContext": {
        "isRedelivery": false
      }
    },
     */
    public string Type { get; set; }
    public LineMessageDto Messages { get; set; }
    public int TimeStamp  { get; set; }
    public LineSourceDto Source { get; set; }
    public string ReplyToken { get; set; }
    public string Mode { get; set; }
    public string WebhookEventId { get; set; }
    public LineDeliveryContextDto DeliveryContext { get; set; }
    
}