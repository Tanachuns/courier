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
    /*
     * curl -v -X POST https://api.line.me/v2/bot/message/reply \
-H 'Content-Type: application/json' \
-H 'Authorization: Bearer {channel access token}' \
-d '{
    "replyToken":"nHuyWiB7yP5Zw52FIkcQobQuGDXCTA",
    "messages":[
        {
              "type":"text",
              "text":"Hello, user"
          },
          {
              "type":"text",
              "text":"May I help you?"
          }
      ]
  }'
     */
    private readonly string _baseUrl;
    private readonly string _channelAccessToken;
    private string _replyToken;
    public SendService()
    {
        Env.Load();
        _baseUrl = Environment.GetEnvironmentVariable("LINE_CHANNEL_ACCESS_TOKEN")??"";
        _channelAccessToken = Environment.GetEnvironmentVariable("LINE_BASEURL")??"";
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
          HttpResponseMessage response = await client.PostAsync("/reply",content);
          
          response.EnsureSuccessStatusCode();
          string result = response.Content.ReadAsStringAsync().Result;
          return result; 
        }
        
    }
}