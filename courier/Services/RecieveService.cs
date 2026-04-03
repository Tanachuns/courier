using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using courier.Interfaces;
using courier.Models.Dto;
using DotNetEnv;
using Newtonsoft.Json;
using Serilog;

namespace courier.Services;

public class ReceiveService : IRecieveService
{
    public bool ValidateSignature(string? body ,string signature)
    {
        try
        {
           Env.Load();
            var secret = Environment.GetEnvironmentVariable("LINE_SECRET") ?? "";
            byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
    
            byte[] messageBytes = Encoding.UTF8.GetBytes(body);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(messageBytes);
                string computedSignature = Convert.ToBase64String(hashBytes);
                return  CryptographicOperations.FixedTimeEquals(
                    Encoding.UTF8.GetBytes(computedSignature), 
                    Encoding.UTF8.GetBytes(signature)
                );
            }
        }
        catch 
        {
            return false;
        }
    }
}