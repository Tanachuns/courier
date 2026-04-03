using Microsoft.AspNetCore.Mvc;

namespace courier.Controllers;

public class HookController : Controller
{
    [HttpGet(Name = "HealthCheck")]
    public IActionResult Index()
    {
        return Ok(new {status="Fine"});
    }
}