using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FMS.Data.Models;
using FMS.Data.Services;
using Microsoft.AspNetCore.Authorization;
using FMS.Web.Models;


namespace FMS.Web.Controllers
{
    [Authorize]
    public class SponsorshipsController : BaseController
    {
            // provide suitable controller actions
            private IRehomingService svc;
            
            //dependency injection used here
            public SponsorshipsController(IRehomingService ss)
            {
                svc = ss;
            }

            
            // GET /Sponsorship options
            public IActionResult Index()
            {   
                
                return View();
                
            }
        
    }
}