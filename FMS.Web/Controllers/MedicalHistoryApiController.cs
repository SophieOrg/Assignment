using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

using FMS.Data.Models;
using FMS.Data.Services;
using FMS.Web.Models;

namespace FMS.Web.Controllers
{
    [ApiController]  
    [Route("api/ticket")]
    public class MedicalHistoryApiController : BaseController
    {
        private readonly IRehomingService svc;

        public MedicalHistoryApiController(IRehomingService ss)
        {
            svc = ss; // initialise via dependency injection
        } 

        // GET /ticket/index
        [HttpGet]
        public IActionResult Index()
        {
            // return open tickets
            var tickets = svc.GetOpenTickets();
            var response = tickets.Select(t => t.ToDto() ).ToList(); 

            return Ok(response); 
        }
     
        [HttpGet("search")]
        public IActionResult Search(string query = "", TicketRange range = TicketRange.ALL)
        {                             
            var tickets = svc.SearchTickets(range, query);
            var results = tickets.Select( t => t.ToDto() ).ToList();
            
            // return custom results list
            return Ok(results);
        }        
             
        // GET/ticket/{id}
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var ticket = svc.GetTicket(id);
            if (ticket == null)
            {
                return NotFound();             
            }
            // return custom ticket object
            return Ok(ConvertToCustomTicketObject(ticket));
        }

        // POST /ticket/close/{id}
        [HttpPost]       
        public IActionResult Close([Bind("Id, Resolution")] Ticket t)
        {
            // close ticket via service
            var ticket = svc.CloseTicket(t.Id, t.Resolution);           

            // return updated ticket
            return Ok(ticket);
        }
       
        // POST /ticket/create
        [HttpPost]
        public IActionResult Create(TicketCreateViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                var ticket = svc.CreateTicket(tvm.StudentId, tvm.Issue);
                return Ok(ticket);
            }
            
            // 
            return NotFound();
        }


        private object ConvertToCustomTicketObject(Ticket t)
        {
            return new {   
                Id = t.Id,
                Issue = t.Issue, 
                CreatedOn = t.CreatedOn.ToShortDateString(),
                Active = t.Active,
                Resolution = t.Resolution,
                Student = t.Student?.Name
            };
        }

    }
}
