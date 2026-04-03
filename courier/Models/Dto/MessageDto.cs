namespace courier.Models.Dto;

public class MessageDto
{
    public string message { get; set; }
    public string type { get; set; }

    public MessageDto(string _message, string _type)
    {
        message = _message;
        type = _type;
    }
}