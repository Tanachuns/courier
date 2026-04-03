namespace courier.Interfaces;

public interface ISendService
{
    public void SetReplyToken(string replyToken);
    public Task<string> Send(string message,string type);
}