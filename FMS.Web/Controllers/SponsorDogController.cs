using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FMS.Data.Models;
using FMS.Data.Services;
using Microsoft.AspNetCore.Authorization;
using FMS.Web.Models;


namespace FMS.Web.Controllers
{
    [Authorize]
    public class SponsorDogController : BaseController
    {
            // provide suitable controller actions
            private IRehomingService svc;
            
            //dependency injection is used here
            public SponsorDogController(IRehomingService ss)
            {
                svc = ss;
            }

            
            // GET /Dog
            public IActionResult Index()
            {   
                
               var dog = svc.GetSponsorDogs()
                            .ToList();
               return View(dog);
                
            }

            //GET /Dog/ Search fucntion
            public IActionResult Search(string searchBreed)
            {
                var dog = svc.GetSponsorDogs()
                             .Where(v => v.Breed == searchBreed)
                             .ToList();
                
                if (dog.Count == 0)
                {
                    //suitable warning alert and redirect
                    Alert($"No dog matching your search result was found.", AlertType.warning);
                    return RedirectToAction(nameof(Index));
                }

                return View(dog);
            }


            
            // GET /dog/details/{id}
            public IActionResult Details(int id)
            {  
                // retrieve the dog with specifed id from the service
                var v = svc.GetSponsorDog(id);

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
            public IActionResult Create([Bind("Breed,Name,ChipNumber,Age,PhotoUrl")]
                                         SponsorDog v)
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
                    v = svc.AddSponsorDog(v.Breed,v.Name,v.ChipNumber,v.Age,v.ReasonForSponsor,v.PhotoUrl);
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
                var v = svc.GetSponsorDog(id);

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
            public IActionResult Edit(int id, SponsorDog v)
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
                    svc.UpdateSponsorDog(v);      
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
                var v = svc.GetSponsorDog(id);
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
                svc.DeleteSponsorDog(id);

                Alert("Dog deleted successfully.", AlertType.info);
                // redirect to the index view
                return RedirectToAction(nameof(Index));
            }

        }
    }