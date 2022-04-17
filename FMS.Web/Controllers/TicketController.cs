using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

using FMS.Data.Models;
using FMS.Data.Services;
using FMS.Web.Models;

namespace SMS.Web.Controllers
{
    [Authorize]
    public class TicketController : BaseController
    {   
        
        private readonly IFleetService svc;
        public TicketController()
        {
            svc = new StudentServiceDb();
        } 

        // GET /ticket/index
        public IActionResult Index()
        {
            // return open tickets
            var tickets = svc.GetAllMots();

            return View(tickets);
        }
          
             
        // GET/ticket/{id}
        public IActionResult Details(int id)
        {
            var ticket = svc.GetMot(id);
            if (ticket == null)
            {
                Alert("Ticket Not Found", AlertType.warning);  
                return RedirectToAction(nameof(Index));             
            }

            return View(ticket);
        }

        // GET /ticket/create
        [Authorize(Roles="admin,manager")]
        public IActionResult Create()
        {
            var vehicles = svc.GetVehicles();
            // populate viewmodel select list property
            var tvm = new TicketCreateViewModel {
                Vehicles = new SelectList(vehicles,"Id","Make") 
            };
            
            // render blank form
            return View( tvm );
        }
       
        // POST /ticket/create
        [HttpPost]
        [Authorize(Roles="admin,manager")]
        public IActionResult Create(TicketCreateViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                svc.CreateTicket(tvm.StudentId, tvm.Issue);
     
                Alert($"Ticket Created", AlertType.info);  
                return RedirectToAction(nameof(Index));
            }
            
            // redisplay the form for editing
            return View(tvm);
        }

    }
}