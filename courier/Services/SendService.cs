using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using courier.Models.Dto;
using Newtonsoft.Json;

namespace courier.Services;

public class SendService
{
    private HookRequestDto Request;

    public SendService(HookRequestDto _request)
    {
        Request= _request;
    }

    public  bool ValidateSignature()
    {
        var body = JsonConvert.SerializeObject(Request);
        var secret = Environment.GetEnvironmentVariable("LINE_SECRET");
        var keyBytes = Encoding.UTF8.GetBytes(secret);
        var bodyBytes = Encoding.UTF8.GetBytes(body);
        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(bodyBytes);
        var signature = Convert.ToBase64String(hash);
        return secret == signature;
    }
}