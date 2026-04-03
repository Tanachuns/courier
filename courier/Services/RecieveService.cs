using System.Security.Cryptography;
using System.Text;
using courier.Interfaces;
using courier.Models.Dto;
using Newtonsoft.Json;

namespace courier.Services;

public class RecieveService : IRecieveService
{
    public  bool ValidateSignature(HookRequestDto request,string signature)
    {
        var body = JsonConvert.SerializeObject(request);
        var secret = Environment.GetEnvironmentVariable("LINE_SECRET");
        var keyBytes = Encoding.UTF8.GetBytes(secret);
        var bodyBytes = Encoding.UTF8.GetBytes(body);
        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(bodyBytes);
        var base64String = Convert.ToBase64String(hash);
        return signature != base64String;
    }
}