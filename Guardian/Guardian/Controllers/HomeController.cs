using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Guardian.Models;
using Microsoft.AspNetCore.Authorization;

namespace Guardian.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Home";
        ViewBag.Name = HttpContext.Session.GetString("Name");
        return View();
    }
  
}