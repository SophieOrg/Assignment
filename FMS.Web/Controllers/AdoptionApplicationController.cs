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
        // reduces coupling between controllers and the service implementation
        public AdoptionApplicationController(IRehomingService ss)
        {
            svc = ss;
        } 

        // GET /adoption application/index
        public IActionResult Index(AdoptionApplicationSearchViewModel m)
        {           
             // set the viewmodel AdoptionApplications property by calling service method 
            // using the range and query values from the viewmodel 
            m.AdoptionApplications = svc.SearchAdoptionApplications(m.AdoptionApplicationRange, m.Query);

            return View(m);
        } 

               
        // GET/adoption application/{id}
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

        // POST /adoption application/approve/{id}
        [HttpPost]
        [Authorize(Roles="admin,manager")]
        public IActionResult Approve([Bind("Id")] AdoptionApplication t)
        {
            // approve adoption application via service
            var adoptionapplication = svc.ApproveAdoptionApplication(t.Id);
            if (adoptionapplication == null)
            {
                Alert("Adoption Application not found.", AlertType.warning);                               
            }
            else
            {
                Alert($"Adoption Application {t.Id } approved.", AlertType.info);
                /*svc.DeleteDog(t.Id+1); */
            }

            // redirect to the index view
            return RedirectToAction("SendGridAdoptionEmail", "Home");
        }
       
        // GET /adoption application/create
        [Authorize(Roles="guest")]
        public IActionResult CreateApplication()
        {
            var dogs = svc.GetDogs();
            // populate viewmodel select list property
            var tvm = new AdoptionApplicationCreateViewModel
            {
                Dogs = new SelectList(dogs,"Id","Name") 
            };
            
            // render blank form
            return View( tvm );
        }
       
        // POST /adoption application/create
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


         //GET /dog/AdoptionApplicationDelete/ {id}

        public IActionResult AdoptionApplicationDelete(int id)
        {
            var adoptionapplication = svc.GetAdoptionApplication(id);

            if(adoptionapplication == null)
            {
                Alert ($"Adoption application {id} not found.", AlertType.warning);
                
                //redirect to index view
                return RedirectToAction(nameof(Index));
            }

            return View (adoptionapplication);
        }

        //POST /dog/adoptionapplicationdeleteconfirm/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdoptionApplicationDeleteConfirm(int id, int dogId)
        {
            svc.DeleteAdoptionApplication(id);
            Alert($"Adoption application deleted successfully for dog {dogId}.", AlertType.info);
            
            //redirect to index view
            return RedirectToAction(nameof(Index));
        }

    }
}
