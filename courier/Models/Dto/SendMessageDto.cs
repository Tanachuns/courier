namespace courier.Models.Dto;

public class SendMessageDto
{
    // {
    //     "replyToken":"nHuyWiB7yP5Zw52FIkcQobQuGDXCTA",
    //     "messages":[
    //     {
    //         "type":"text",
    //         "text":"Hello, user"
    //     },
    //     {
    //         "type":"text",
    //         "text":"May I help you?"
    //     }
    //     ]
    // }
    
    public string replyToken { get; set; }
    public MessageDto[] messages { get; set; }
}