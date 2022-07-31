﻿using System.Diagnostics;
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

     public IActionResult Quote()
    {
        return View();
    }
    
    //GET: /Home/Contact
     public IActionResult Contact()
    {   
        //display a blank form to allow users to contact the rehoming centre
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
