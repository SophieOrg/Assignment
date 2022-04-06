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
        return View();
    }


    public IActionResult About()
    {
        var about = new AboutViewModel {
            Title = "About",
            Message = "Our mission is to develop great solutions for Fleet management",
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
