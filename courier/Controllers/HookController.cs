using courier.Interfaces;
using courier.Models.Dto;
using courier.Models.Http;
using courier.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace courier.Controllers;

public class HookController : Controller
{

    private readonly IRecieveService _recieveService;
    private readonly ISendService _sendService;
    public HookController(IRecieveService recieveService,ISendService sendService) 
    {
        _recieveService = recieveService;
        _sendService = sendService;
        
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
            if (string.IsNullOrEmpty(signature)||!_recieveService.ValidateSignature(rawBody,signature))
            {
                response.IsSuccess = false;
                response.Message = "Invalid signature";
                Log.Error(response.Message);
                return BadRequest(response);
            }

            HookRequestDto? requestDto = JsonConvert.DeserializeObject<HookRequestDto>(request.ToString() ?? "");
            if (requestDto != null)
            {
                foreach (LineEventDto eventDto in requestDto.events)
                {
                    var arg = new ArguementService(eventDto);
                    // SendService.SetReplyToken(eventDto.replytoken);
                    // var result = await SendService.Send("Ok jaa", "text");
                    // Log.Information(result);
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