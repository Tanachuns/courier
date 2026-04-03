using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult Post()
    {
        try
        {
            Log.Information("hooked");
            return Ok(new { status = "hooked" });
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return StatusCode(500, e.Message);
        }
    }
}