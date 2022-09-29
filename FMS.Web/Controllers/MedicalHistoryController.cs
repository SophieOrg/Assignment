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

        // GET /medical history/index
        public IActionResult Index(MedNoteSearchViewModel m)
        {                  
            // set the viewmodel MedicalNotes property by calling service method 
            // using the range and query values from the viewmodel 
            m.MedicalNotes = svc.SearchMedicalHistoryNotes(m.MedNoteRange, m.Query);

            return View(m);
        }       
               
        // GET/medical history note/{id}
        public IActionResult Details(int id)
        {
            var medNote = svc.GetMedicalHistory(id);
            if (medNote == null)
            {
                Alert("Medical history note not found.", AlertType.warning);  
                return RedirectToAction(nameof(Index));             
            }

            return View(medNote);
        }

        // POST /medical history note/close/{id}
        [HttpPost]
        [Authorize(Roles="volunteer,manager")]
        public IActionResult Close([Bind("Id, Resolution")] MedicalHistory t)
        {
            // close medical history note via service
            var medNote = svc.CloseMedicalHistoryNote(t.Id, t.Resolution);
            if (medNote == null)
            {
                Alert("Medical history note not found.", AlertType.warning);                               
            }
            else
            {
                Alert($"Medical history note {t.Id } closed.", AlertType.info);  
            }

            // redirect to the index view
            return RedirectToAction(nameof(Index));
        }
       
        // GET /medical history note/create
        [Authorize(Roles="volunteer,manager")]
        public IActionResult Create()
        {
            var dogs = svc.GetDogs();
            // populate viewmodel select list property
            var tvm = new MedNoteCreateViewModel {
                Dogs = new SelectList(dogs,"Id","Name") 
            };
            
            // render blank form
            return View( tvm );
        }
       
        // POST /medical history note/create
        [HttpPost]
        [Authorize(Roles="volunteer,manager")]
        public IActionResult Create(MedNoteCreateViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                var medNote = svc.CreateMedicalHistory(tvm.DogId,tvm.CreatedOn,tvm.Medication,tvm.Report);
                if (medNote != null)
                {
                    Alert($"Medical history note created.", AlertType.info);  
                }
                else
                {
                    Alert($"Problem creating medical note", AlertType.warning);
                }
                
                return RedirectToAction(nameof(Index));
            }
            
            // redisplay the form for editing
            return View(tvm);
        }

    }
}
