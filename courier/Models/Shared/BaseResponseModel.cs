namespace courier.Models.Http;

public class BaseResponseModel
{
    public bool isSuccess { get; set; }
    public string message { get; set; }
    public object data { get; set; }
}