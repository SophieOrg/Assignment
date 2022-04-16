using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FMS.Web.Models;

namespace FMS.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.LongTime = DateTime.Now.ToLongTimeString();
        ViewBag.Message = "Time Now";
        return View();
    }
    
     public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        var about = new AboutViewModel 
        {
            Title = "About Our Company - Fleet Management",
            Message = "Our mission is to develop an organised and easy to use service for Fleet management",
            Formed = new DateTime(2000,10,1)
        };
        return View(about);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
