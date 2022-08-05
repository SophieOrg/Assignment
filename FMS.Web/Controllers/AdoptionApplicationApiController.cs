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
    public class AdoptionApplicationApiController : BaseController
    {
        private readonly IRehomingService svc;

        public AdoptionApplicationApiController(IRehomingService ss)
        {
            svc = ss; // initialise via dependency injection
        } 

        // GET /ticket/index
        [HttpGet]
        public IActionResult Index()
        {
            // return open tickets
            var tickets = svc.GetValidAdoptionApplications();
            var response = tickets.Select(t => t.ToDto() ).ToList(); 

            return Ok(response); 
        }
     
        [HttpGet("search")]
        public IActionResult Search(string query = "", AdoptionApplicationRange AdoptionApplicationRange = AdoptionApplicationRange.ALL)
        {                             
            var tickets = svc.SearchAdoptionApplications(AdoptionApplicationRange, query);
            var results = tickets.Select( t => t.ToDto() ).ToList();
            
            // return custom results list
            return Ok(results);
        }        
             
        // GET/ticket/{id}
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var ticket = svc.GetAdoptionApplication(id);
            if (ticket == null)
            {
                return NotFound();             
            }
            // return custom ticket object
            return Ok(ConvertToCustomTicketObject(ticket));
        }

        // POST /ticket/close/{id}
        [HttpPost]       
        public IActionResult Close([Bind("Id, Resolution")] AdoptionApplication t)
        {
            // close ticket via service
            var ticket = svc.CloseAdoptionApplication(t.Id, t.Resolution);           

            // return updated ticket
            return Ok(ticket);
        }
       
        // POST /ticket/create
        [HttpPost]
        public IActionResult Create(AdoptionApplicationCreateViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                var ticket = svc.CreateAdoptionApplication(tvm.DogId,tvm.Name,tvm.Email,tvm.PhoneNumber,tvm.Information);
                return Ok(ticket);
            }
            
            // 
            return NotFound();
        }


        private object ConvertToCustomTicketObject(AdoptionApplication t)
        {
            return new {   
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber,
                Information = t.Information,
                Active = t.Active,
                Resolution = t.Resolution,
                Dog = t.Dog?.Name
            };
        }

    }
}