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

        // GET /adoption application/index
        [HttpGet]
        public IActionResult Index()
        {
            // return awaiting adoption applications
            var adoptionApplications = svc.GetValidAdoptionApplications();
            var response = adoptionApplications.Select(t => t.ToDto() ).ToList(); 

            return Ok(response); 
        }
     
        [HttpGet("search")]
        public IActionResult Search(string query = "", AdoptionApplicationRange AdoptionApplicationRange = AdoptionApplicationRange.ALL)
        {                             
            var adoptionApplications = svc.SearchAdoptionApplications(AdoptionApplicationRange, query);
            var results = adoptionApplications.Select( t => t.ToDto() ).ToList();
            
            // return custom results list
            return Ok(results);
        }        
             
        // GET/adoption application/{id}
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var adoptionApplication = svc.GetAdoptionApplication(id);
            if (adoptionApplication == null)
            {
                return NotFound();             
            }
            // return custom adoption application object
            return Ok(ConvertToCustomTicketObject(adoptionApplication));
        }

        // POST /adoption application/approve/{id}
        [HttpPost]       
        public IActionResult Approve([Bind("Id, Resolution")] AdoptionApplication t)
        {
            // approve adoption application via service
            var adoptionApplication = svc.ApproveAdoptionApplication(t.Id, t.Resolution);           

            // return updated adoption application
            return Ok(adoptionApplication);
        }
       
        // POST /adoption application/create
        [HttpPost]
        public IActionResult Create(AdoptionApplicationCreateViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                var adoptionApplication = svc.CreateAdoptionApplication(tvm.DogId,tvm.Name,tvm.Email,tvm.PhoneNumber,tvm.Information);
                return Ok(adoptionApplication);
            }
            
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