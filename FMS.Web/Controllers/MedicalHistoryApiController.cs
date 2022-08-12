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

        // GET /medical history note/index
        [HttpGet]
        public IActionResult Index()
        {
            // return open tickets
            var medNotes = svc.GetOngoingMedicalHistoryNotes();
            var response = medNotes.Select(t => t.ToDto() ).ToList(); 

            return Ok(response); 
        }
     
        [HttpGet("search")]
        public IActionResult Search(string query = "", TicketRange range = TicketRange.ALL)
        {                             
            var medNotes = svc.SearchMedicalHistoryNotes(range, query);
            var results = medNotes.Select( t => t.ToDto() ).ToList();
            
            // return custom results list
            return Ok(results);
        }        
             
        // GET/medical history note/{id}
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var medNote = svc.GetMedicalHistory(id);
            if (medNote == null)
            {
                return NotFound();             
            }
            // return custom medNote object
            return Ok(ConvertToCustomTicketObject(medNote));
        }

        // POST /medical history note/close/{id}
        [HttpPost]       
        public IActionResult Close([Bind("Id, Resolution")] MedicalHistory t)
        {
            // close medical history note via service
            var medNote = svc.CloseMedicalHistoryNote(t.Id, t.Resolution);           

            // return updated medical history note
            return Ok(medNote);
        }
       
        // POST /medical history note/create
        [HttpPost]
        public IActionResult Create(MedNoteCreateViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                var medNote = svc.CreateMedicalHistory(tvm.DogId,tvm.Medication,tvm.Report);
                return Ok(medNote);
            }
            
            // 
            return NotFound();
        }


        private object ConvertToCustomTicketObject(MedicalHistory t)
        {
            return new {   
                Id = t.Id,
                Report = t.Report, 
                Medication = t.Medication,
                CreatedOn = t.CreatedOn.ToShortDateString(),
                Active = t.Active,
                Resolution = t.Resolution,
                Dog = t.Dog?.Name
            };
        }

    }
}