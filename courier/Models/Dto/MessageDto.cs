namespace courier.Models.Dto;

public class MessageDto
{
    public string text { get; set; }
    public string type { get; set; }

    public MessageDto(string _message, string _type)
    {
        text = _message;
        type = _type;
    }
}