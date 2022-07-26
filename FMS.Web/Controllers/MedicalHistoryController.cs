using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

using FMS.Data.Models;
using FMS.Data.Services;
using FMS.Web.Models;

namespace FMS.Web.Controllers
{
    [Authorize]
    public class MedicalHistoryController : BaseController
    {
        private readonly IRehomingService svc;

        // use constructor dependency injection to initialise service
        public MedicalHistoryController(IRehomingService ss)
        {
            svc = ss;
        } 

        // GET /ticket/index
        public IActionResult Index(TicketSearchViewModel m)
        {                  
            // set the viewmodel Tickets property by calling service method 
            // using the range and query values from the viewmodel 
            m.Tickets = svc.SearchTickets(m.Range, m.Query);

            return View(m);
        }       
               
        // GET/ticket/{id}
        public IActionResult Details(int id)
        {
            var ticket = svc.GetMedicalHistory(id);
            if (ticket == null)
            {
                Alert("Ticket Not Found", AlertType.warning);  
                return RedirectToAction(nameof(Index));             
            }

            return View(ticket);
        }

        // POST /ticket/close/{id}
        [HttpPost]
        [Authorize(Roles="admin,manager")]
        public IActionResult Close([Bind("Id, Resolution")] MedicalHistory t)
        {
            // close ticket via service
            var ticket = svc.CloseTicket(t.Id, t.Resolution);
            if (ticket == null)
            {
                Alert("Ticket Not Found", AlertType.warning);                               
            }
            else
            {
                Alert($"Ticket {t.Id } closed", AlertType.info);  
            }

            // redirect to the index view
            return RedirectToAction(nameof(Index));
        }
       
        // GET /ticket/create
        [Authorize(Roles="admin,manager")]
        public IActionResult Create()
        {
            var students = svc.GetDogs();
            // populate viewmodel select list property
            var tvm = new TicketCreateViewModel {
                Dogs = new SelectList(students,"Id","Name") 
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
                svc.CreateMedicalHistory(tvm.DogId,tvm.On,tvm.Vet,tvm.Status,tvm.Report);
     
                Alert($"Ticket Created", AlertType.info);  
                return RedirectToAction(nameof(Index));
            }
            
            // redisplay the form for editing
            return View(tvm);
        }

    }
}
