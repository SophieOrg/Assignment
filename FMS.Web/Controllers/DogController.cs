using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FMS.Data.Models;
using FMS.Data.Services;
using Microsoft.AspNetCore.Authorization;
using FMS.Web.Models;


namespace FMS.Web.Controllers
{
    [Authorize]
    public class DogController : BaseController
    {
            // provide suitable controller actions
            private IRehomingService svc;
            
            //dependency injection is used here
            public DogController(IRehomingService ss)
            {
                svc = ss;
            }

            
            // GET /Dog
            public IActionResult Index()
            {   
                
               var dog = svc.GetDogs()
                                .ToList();
               return View(dog);
                
            }

            
            // GET /dog/details/{id}
            public IActionResult Details(int id)
            {  
                // retrieve the dog with specifed id from the service
                var v = svc.GetDog(id);

                // if dog not found(i.e. GetDog() returns null), display an alert & redirect to index
                //else display the dog with that id in dog details view
                if (v == null)
                {
                    //suitable warning alert and redirect
                    Alert($"Dog {id} not found.", AlertType.warning);
                    return RedirectToAction(nameof(Index));
                }

                // pass dog as parameter to the dogs detail view
                return View(v);
            }

            // GET: /dog/create
            [Authorize(Roles="admin,manager")]
            public IActionResult Create()
            {   
                // display blank form to create a new dog
                return View();
            }

            // POST /dog/create
            [HttpPost]
            [ValidateAntiForgeryToken]
            [Authorize(Roles="admin,manager")]
            public IActionResult Create([Bind("Breed,Name,ChipNumber,DOB,PhotoUrl")]
                                         Dog v)
            {
                // check chip number is unique for this dog
                if (svc.IsDuplicateDogChipped(v.ChipNumber, v.Id))
                {
                    // add manual validation error
                    ModelState.AddModelError(nameof(v.ChipNumber),"This chip number is already in use.");
                }

                // complete POST action to add dog
                if (ModelState.IsValid)
                {
                    // pass data to service to store 
                    v = svc.AddDog(v.Breed,v.Name,v.ChipNumber,v.DOB,v.Information,v.PhotoUrl);
                    Alert($"New dog created successfully.", AlertType.success);

                    return RedirectToAction(nameof(Index), new { Id = v.Id});
                }
                
                // redisplay the form for editing as there are validation errors
                return View(v);
            }

            // GET /dog/edit/{id}
            [Authorize(Roles="admin,manager")]
            public IActionResult Edit(int id)
            {        
                // load the dog using the service
                var v = svc.GetDog(id);

                // check if v is null and if so alert
                if (v == null)
                {
                    Alert($"Dog {id} not found.", AlertType.warning);
                    return RedirectToAction(nameof(Index));
                }   

                // pass dog to view for editing
                return View(v);
            }

            // POST /dog/edit/{id}
            [HttpPost]
            [ValidateAntiForgeryToken]
            [Authorize(Roles="admin,manager")]
            public IActionResult Edit(int id, Dog v)
            {
                // check chip number is unique for this dog
                if (svc.IsDuplicateDogChipped(v.ChipNumber,v.Id))
                {
                    // add manual validation error
                    ModelState.AddModelError("Chip Number","This Chip Number is already registered on the system.");
                }

                // validate and complete POST action to save dog changes
                if (ModelState.IsValid)
                {
                    // pass data to service to update
                    svc.UpdateDog(v);      
                    Alert("Dog updated successfully.", AlertType.info);

                    return RedirectToAction(nameof(Index), new { Id = v.Id });
                }

                // redisplay the form for editing as validation errors
                return View(v);
            }

            // GET / dog /delete/{id}
            [Authorize(Roles="admin,manager")]      
            public IActionResult Delete(int id)
            {       
                // load the dog using the service
                var v = svc.GetDog(id);
                // check the returned dog is not null and if so return alert and redirect to index page
                if (v == null)
                {
                    //suitable warning alert and redirection to index page
                    Alert($"Dog {id} not found.", AlertType.warning);
                    return RedirectToAction(nameof(Index));
                }     
                
                // pass dog to view for deletion confirmation
                return View(v);
            }

            // POST /dog/delete/{id}
            [HttpPost]
            [Authorize(Roles="admin,manager")]
            [ValidateAntiForgeryToken]              
            public IActionResult DeleteConfirm(int id)
            {
                // delete dog via service
                svc.DeleteDog(id);

                Alert("Dog deleted successfully.", AlertType.info);
                // redirect to the index view
                return RedirectToAction(nameof(Index));
            }


            // ============== Dog Medical History Management ==============

            // GET /dog/Create Medcial History Note/{id}
            public IActionResult TicketCreate(int id)
            {     
                var v = svc.GetDog(id);
                // check the returned dog is not null and if so alert
                if (v == null)
                {
                    Alert($"Dog {id} not found.", AlertType.warning);
                    return RedirectToAction(nameof(Index));
                }

                // create a medical history note view model and set DogId (foreign key)
                var medhistorynote = new MedicalHistory { DogId = id }; 

                return View( medhistorynote );
            }

            // POST /dog/medicalhistorynotecreate
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult TicketCreate(MedicalHistory m)
            {
                if (ModelState.IsValid)
                {                
                    var mot = svc.CreateMedicalHistory(m.DogId,m.Medication,m.Report);
                    Alert($"Medical history note created successfully for dog {m.DogId}.", AlertType.info);
                    return RedirectToAction(nameof(Details), new { Id = m.DogId });
                }
                // redisplay the form for editing
                return View(m);
            }

            // GET /dog/MedicalHistoryNoteDelete/{id}
            public IActionResult TicketDelete(int id)
            {
                // load the Medical History Note using the service
                var medhistorynote = svc.GetMedicalHistory(id);
                // check the returned Medical History Note is not null and if so return alert & redirect to index
                if (medhistorynote == null)
                {
                    Alert($"Medical history note {id} not found.", AlertType.warning);
                    return RedirectToAction(nameof(Index));
                }     
                
                // pass medicalhistorynote to view for deletion confirmation
                return View(medhistorynote);
            }

            // POST /dog/ticketdeleteconfirm/{id}
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult TicketDeleteConfirm(int id, int dogId)
            {
                // delete Medical History note via service
                svc.DeleteMedicalHistoryNote(id);
                Alert($"Medical history note deleted successfully for dog {dogId}.", AlertType.info);
                
                // redirect to the MOT ticket index view
                return RedirectToAction(nameof(Details), new { Id = dogId });
            }

        }
    }