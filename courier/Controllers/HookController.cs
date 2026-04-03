using System.Net;
using courier.Models.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace courier.Controllers;

public class HookController : Controller
{
    [HttpGet(Name = "HealthCheck")]
    public IActionResult HealthCheck()
    {
        return Ok(new {status="Fine"});
    }
    
    [HttpPost(Name = "Webhook")]
    public IActionResult Index([FromBody]HookRequestDto tt)
    {
        try
        {
            var st = JsonConvert.SerializeObject(tt);
            Log.Information("hooked");            
            Log.Information(st);
            return Ok(tt);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return StatusCode(500, e.Message);
        }
    }
}