using System.Net;
using System.Text;
using courier.Interfaces;
using courier.Models.Dto;
using courier.Models.Http;
using courier.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace courier.Controllers;

public class HookController(IRecieveService recieveService) : Controller
{
    [HttpGet(Name = "HealthCheck")]
    public IActionResult HealthCheck()
    {
        bool isUnhealthy = false;
        DotNetEnv.Env.Load();
        var secret = Environment.GetEnvironmentVariable("LINE_SECRET");
        if (string.IsNullOrEmpty(secret))
        {
            Log.Error("Invalid secret environment variable");
            isUnhealthy = true;
        }
        
        if (isUnhealthy)
        {
            return StatusCode(503, "You are unhealthy");
        }
        
        return Ok(new {status="Fine"});
    }
    
    [HttpPost(Name = "Webhook")]
    public async Task<IActionResult> Index([FromBody] HookRequestDto request)
    {
        BaseResponseModel response = new BaseResponseModel();
        try
        {
            Log.Information("Starting webhook");
            string rawBody = JsonConvert.SerializeObject(request);
            Log.Information("request: " + rawBody);
            var signature = Request.Headers["x-line-signature"].FirstOrDefault();
            Request.EnableBuffering();
           
            Log.Information("signature: " + signature);
            if (string.IsNullOrEmpty(signature)||!recieveService.ValidateSignature(rawBody,signature))
            {
                Log.Error("Invalid signature");
                response.isSuccess = false;
                response.message = "Invalid signature";
                return BadRequest(response);
            }
            Log.Information("Happy");
            response.isSuccess = true;
            response.data = null;
            return Ok(response);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return StatusCode(500, e.Message);
        }
    }
}