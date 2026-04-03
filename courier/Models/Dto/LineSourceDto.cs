namespace courier.Models.Dto;

public class LineSourceDto
{
    // "source": {
    //     "type": "user",
    //     "userId": "U80696558e1aa831..."
    //   },
    
    public string Type { get; set; }
    public string UserId { get; set; }
    public string GroupId { get; set; }
}