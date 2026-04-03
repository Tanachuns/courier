using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using courier.Interfaces;
using courier.Models.Dto;
using DotNetEnv;
using Newtonsoft.Json;

namespace courier.Services;

public class SendService : ISendService
{
    private readonly string _baseUrl;
    private readonly string _channelAccessToken;
    private string _replyToken;
    public SendService()
    {
        Env.Load();
        _baseUrl = Environment.GetEnvironmentVariable("LINE_BASEURL")??"";
        _channelAccessToken = Environment.GetEnvironmentVariable("LINE_CHANNEL_ACCESS_TOKEN")??"";
    }

    public void SetReplyToken(string replyToken)
    {
        _replyToken = replyToken;
    }

    public async Task<string> Send(string message,string type)
    {
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(_baseUrl);
          SendMessageDto messageDto = new SendMessageDto();
          messageDto.replyToken = _replyToken;
          messageDto.messages =  new MessageDto[1]{new MessageDto(message,type)};
          HttpContent content = new StringContent( JsonConvert.SerializeObject(messageDto), Encoding.UTF8, "application/json");
          client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _channelAccessToken);
          HttpResponseMessage response = await client.PostAsync("reply",content);
          string result = response.Content.ReadAsStringAsync().Result;
          
          if (!response.IsSuccessStatusCode)
          {
             throw  new Exception(result);
          }
          return result; 
        }
    }
}