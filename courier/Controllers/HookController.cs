using System.Net;
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
    public IActionResult Post(object tt)
    {
        try
        {
            var st = JsonConvert.SerializeObject(tt);
            Log.Information("hooked");            
            Log.Information(st);
            return Ok(new { status = "hooked" });
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return StatusCode(500, e.Message);
        }
    }
}