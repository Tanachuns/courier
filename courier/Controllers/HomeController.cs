using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using courier.Models;

namespace courier.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Ok("Home Page");
    }
}