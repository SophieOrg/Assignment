using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FMS.Data.Models;
using FMS.Data.Services;
using Microsoft.AspNetCore.Authorization;
using FMS.Web.Models;

namespace FMS.Web.Controllers
{
    [Authorize]
    public class VehicleController : BaseController
    {
            // provide suitable controller actions
            private IFleetService svc;

            public VehicleController(IFleetService ss)
            {
                svc = ss;
            }


            // GET /Vehicle
            public IActionResult Index()
            {   
                
                return View(svc.GetVehicles()
                               .OrderBy(x=>x.MotDue)
                               .ToList());
                
            }
            

            // GET /vehicle/details/{id}
            public IActionResult Details(int id)
            {  
                // retrieve the vehicle with specifed id from the service
                var v = svc.GetVehicle(id);

                // if vehicle not found(i.e. GetVehicle() returns null), display an alert & redirect to index
                //else display the vehicle with that id in vehicle details view
                if (v == null)
                {
                    //suitable warning alert and redirect
                    Alert($"Vehicle {id} not found", AlertType.warning);
                    return RedirectToAction(nameof(Index));
                }

                // pass vehicle as parameter to the vehicles detail view
                return View(v);
            }

            // GET: /vehicle/create
            [Authorize(Roles="admin,manager")]
            public IActionResult Create()
            {   
                // display blank form to create a new vehicle
                return View();
            }

            // POST /vehicle/create
            [HttpPost]
            [ValidateAntiForgeryToken]
            [Authorize(Roles="admin,manager")]
            public IActionResult Create([Bind("Make,Model,Year,Registration,FuelType,BodyType,TransmissionType,CC,No0fDoors,MotDue,PhotoUrl")]  Vehicle v)
            {
                // check registration is unique for this vehicle
                if (svc.IsDuplicateVehicleReg(v.Registration, v.Id))
                {
                    // add manual validation error
                    ModelState.AddModelError(nameof(v.Registration),"The registration number is already in use");
                }

                // complete POST action to add vehicle
                if (ModelState.IsValid)
                {
                    // pass data to service to store 
                    v = svc.AddVehicle(v.Make, v.Model,v.Year,v.Registration,v.FuelType,v.BodyType,v.TransmissionType,v.CC,v.No0fDoors,v.MotDue,v.PhotoUrl);
                    Alert($"New vehicle created successfully", AlertType.success);

                    return RedirectToAction(nameof(Index), new { Id = v.Id});
                }
                
                // redisplay the form for editing as there are validation errors
                return View(v);
            }

            // GET /vehicle/edit/{id}
            [Authorize(Roles="admin,manager")]
            public IActionResult Edit(int id)
            {        
                // load the vehicle using the service
                var v = svc.GetVehicle(id);

                // check if v is null and if so alert
                if (v == null)
                {
                    Alert($"Vehicle {id} not found", AlertType.warning);
                    return RedirectToAction(nameof(Index));
                }   

                // pass vehicle to view for editing
                return View(v);
            }

            // POST /vehicle/edit/{id}
            [HttpPost]
            [ValidateAntiForgeryToken]
            [Authorize(Roles="admin,manager")]
            public IActionResult Edit(int id, Vehicle v)
            {
                // check registration number is unique for this student  
                if (svc.IsDuplicateVehicleReg(v.Registration,v.Id))
                {
                    // add manual validation error
                    ModelState.AddModelError("Registration","This registration number is already registered on the system");
                }

                // validate and complete POST action to save vehicle changes
                if (ModelState.IsValid)
                {
                    // pass data to service to update
                    svc.UpdateVehicle(v);      
                    Alert("Vehicle updated successfully", AlertType.info);

                    return RedirectToAction(nameof(Index), new { Id = v.Id });
                }

                // redisplay the form for editing as validation errors
                return View(v);
            }

            // GET / vehicle/delete/{id}
            [Authorize(Roles="admin,manager")]      
            public IActionResult Delete(int id)
            {       
                // load the vehicle using the service
                var v = svc.GetVehicle(id);
                // check the returned vehicle is not null and if so return alert nd redirect to index page
                if (v == null)
                {
                    //suitable warning alert and redirection to index page
                    Alert($"Vehicle {id} not found", AlertType.warning);
                    return RedirectToAction(nameof(Index));
                }     
                
                // pass vehicle to view for deletion confirmation
                return View(v);
            }

            // POST /vehicle/delete/{id}
            [HttpPost]
            [Authorize(Roles="admin,manager")]
            [ValidateAntiForgeryToken]              
            public IActionResult DeleteConfirm(int id)
            {
                // delete vehicle via service
                svc.DeleteVehicle(id);

                Alert("Vehicle deleted successfully", AlertType.info);
                // redirect to the index view
                return RedirectToAction(nameof(Index));
            }


            // ============== Vehicle MOT history management ==============

            // GET /vehicle/CreateMot/{id}
            public IActionResult TicketCreate(int id)
            {     
                var v = svc.GetVehicle(id);
                // check the returned vehicle is not null and if so alert
                if (v == null)
                {
                    Alert($"Vehicle {id} not found", AlertType.warning);
                    return RedirectToAction(nameof(Index));
                }

                // create a ticket view model and set VehicleId (foreign key)
                var mot = new Mot { VehicleId = id }; 

                return View( mot );
            }

            // POST /vehicle/create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult TicketCreate(Mot m)
            {
                if (ModelState.IsValid)
                {                
                    var mot = svc.CreateMot(m.VehicleId,m.On,m.MotTester,m.Status,m.Mileage, m.Report);
                    Alert($"Ticket created successfully for vehicle {m.VehicleId}", AlertType.info);
                    return RedirectToAction(nameof(Details), new { Id = m.VehicleId });
                }
                // redisplay the form for editing
                return View(m);
            }

            // GET /vehicle/ticketdelete/{id}
            public IActionResult TicketDelete(int id)
            {
                // load the MOT ticket using the service
                var mot = svc.GetMot(id);
                // check the returned MOT ticket is not null and if so return alert & redirect to index
                if (mot == null)
                {
                    Alert($"MOT {id} not found", AlertType.warning);
                    return RedirectToAction(nameof(Index));
                }     
                
                // pass ticket to view for deletion confirmation
                return View(mot);
            }

            // POST /vehicle/ticketdeleteconfirm/{id}
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult TicketDeleteConfirm(int id, int vehicleId)
            {
                // delete MOT ticket via service
                svc.DeleteMotTicket(id);
                Alert($"MOT ticket deleted successfully for vehicle {vehicleId}", AlertType.info);
                
                // redirect to the MOT ticket index view
                return RedirectToAction(nameof(Details), new { Id = vehicleId });
            }

        }
    }