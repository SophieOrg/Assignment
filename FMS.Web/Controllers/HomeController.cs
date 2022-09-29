using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FMS.Web.Models;
using SendGrid;
using SendGrid.Helpers.Mail;


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
    
    public IActionResult SendgridEmail()
    {
            ViewData["Message"] = "Your application description page.";

            return View();
    }

    public IActionResult SendgridAdoptionEmail()
    {
            ViewData["Message"] = "Your application description page.";

            return View();
    }

        
    [HttpPost]
    public async Task<IActionResult> SendgridEmailSubmit(Emailmodel emailmodel)
    {
        ViewData["Message"] = "Email Sent!!!...";
        Example emailexample = new Example();
        await emailexample.Execute(emailmodel.From, emailmodel.To, emailmodel.Subject, emailmodel.Body
            , emailmodel.Body);

        return View("Index","Home");
    }

    [HttpPost]
    public async Task<IActionResult> SendgridAdoptionEmailSubmit(Emailmodel emailmodel)
    {
        ViewData["Message"] = "Email Sent!!!...";
        Example emailexample = new Example();
        await emailexample.Execute(emailmodel.From, emailmodel.To, emailmodel.Subject, emailmodel.Body
            , emailmodel.Body);

        return View("Index","AdoptionApplication");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Contact()
    {
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

    internal class Example
    {      
        public async Task Execute(string From,string To,string subject,string plainTextContent,string htmlContent)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(From);            
            var to = new EmailAddress(To);           
            htmlContent = "<strong>" + htmlContent +"</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }

}
