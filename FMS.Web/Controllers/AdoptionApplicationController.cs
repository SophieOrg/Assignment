using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

using FMS.Data.Models;
using FMS.Data.Services;
using FMS.Web.Models;

namespace FMS.Web.Controllers
{
    [Authorize]
    public class AdoptionApplicationController : BaseController
    {
        private readonly IRehomingService svc;

        // use constructor dependency injection to initialise service
        public AdoptionApplicationController(IRehomingService ss)
        {
            svc = ss;
        } 

        // GET /ticket/index
        public IActionResult Index(AdoptionApplicationSearchViewModel m)
        {           
             // set the viewmodel Tickets property by calling service method 
            // using the range and query values from the viewmodel 
            m.AdoptionApplications = svc.SearchAdoptionApplications(m.AdoptionApplicationRange, m.Query);

            return View(m);
        }       
               
        // GET/ticket/{id}
        public IActionResult Details(int id)
        {
            var adoptionapplication = svc.GetAdoptionApplication(id);
            if (adoptionapplication == null)
            {
                Alert("Adoption application not found.", AlertType.warning);  
                return RedirectToAction(nameof(Index));             
            }

            return View(adoptionapplication);
        }

        // POST /ticket/approve/{id}
        [HttpPost]
        [Authorize(Roles="admin,manager")]
        public IActionResult Approve([Bind("Id, Resolution")] AdoptionApplication t)
        {
            // close ticket via service
            var adoptionapplication = svc.ApproveAdoptionApplication(t.Id, t.Resolution);
            if (adoptionapplication == null)
            {
                Alert("Adoption Application not found.", AlertType.warning);                               
            }
            else
            {
                Alert($"Adoption Application {t.Id } approved.", AlertType.info);  
            }

            // redirect to the index view
            return RedirectToAction(nameof(Index));
        }

       
        // GET /ticket/create
        [Authorize(Roles="guest")]
        public IActionResult CreateApplication()
        {
            var dogs = svc.GetDogs();
            // populate viewmodel select list property
            var tvm = new AdoptionApplicationCreateViewModel {
                Dogs = new SelectList(dogs,"Id","Name") 
            };
            
            // render blank form
            return View( tvm );
        }
       
        // POST /ticket/create
        [HttpPost]
        [Authorize(Roles="guest")]
        public IActionResult CreateApplication(AdoptionApplicationCreateViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                svc.CreateAdoptionApplication(tvm.DogId,tvm.Name,tvm.Email,tvm.PhoneNumber,tvm.Information);
     
                Alert($"Adoption application created.", AlertType.info);  
                return RedirectToAction(nameof(Index));
            }
            
            // redisplay the form for editing
            return View(tvm);
        }

    }
}
