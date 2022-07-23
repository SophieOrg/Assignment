using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FMS.Data.Models;
using FMS.Data.Services;
using Microsoft.AspNetCore.Authorization;
using FMS.Web.Models;


namespace FMS.Web.Controllers
{
    [Authorize]
    public class SubscriptionController : BaseController
    {
            // provide suitable controller actions
            private IRehomingService svc;
            
            //dependency injection is used here
            public SubscriptionController(IRehomingService ss)
            {
                svc = ss;
            }

            
            // GET /Subscription options
            public IActionResult Index()
            {   
                
                return View();
                
            }
    }
}