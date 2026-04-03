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

public class HookController : Controller
{

    private readonly IRecieveService RecieveService;
    private readonly ISendService SendService;
    public HookController(IRecieveService recieveService,ISendService sendService) 
    {
        RecieveService = recieveService;
        SendService = sendService;
        
    }
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
    public async Task<IActionResult> Index([FromBody] object request)
    {
        BaseResponseModel response = new BaseResponseModel();
        try
        {
            Log.Information("Starting webhook");

            string? rawBody =request.ToString()?.Replace("\r\n","");
            var signature = Request.Headers["x-line-signature"].FirstOrDefault();
            Log.Information("request: " + rawBody);
            Log.Information("signature: " + signature);
            if (string.IsNullOrEmpty(signature)||!RecieveService.ValidateSignature(rawBody,signature))
            {
                response.IsSuccess = false;
                response.Message = "Invalid signature";
                Log.Error(response.Message);
                return BadRequest(response);
            }

            HookRequestDto requestDto = JsonConvert.DeserializeObject<HookRequestDto>(request.ToString());
            if (requestDto != null)
            {
                foreach (var eventDto in requestDto.events)
                {
                    SendService.SetReplyToken(eventDto.replytoken);
                    var result = await SendService.Send("Ok jaa", "text");
                    Log.Information(result);
                }
            }
            
            Log.Information("Happy");
            return Ok("Happy");
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.IsSuccess = false;
            Log.Error(e.Message);
            return StatusCode(500, response);
        }
    }
}