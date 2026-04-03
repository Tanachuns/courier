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
        DotNetEnv.Env.Load();
        var requestString = JsonConvert.SerializeObject(request);
        var secret = Environment.GetEnvironmentVariable("LINE_SECRET");
        try
        {
            var key = Encoding.UTF8.GetBytes(secret);
            var body = Encoding.UTF8.GetBytes(requestString);

            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(body, 0, body.Length);
                var xLineBytes = Convert.FromBase64String(signature);
                return xLineBytes.Equals(hash);
            }
        }
        catch
        {
            return false;
        }
    }
   
}