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

            public VehicleController()
            {
                svc = new FleetServiceDb();
            }

            // GET /Vehicle
            public IActionResult Index()
            {
                // complete this method
                var vehicles = svc.GetVehicle();
                
                return View(vehicles);
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
            [Authorize(Roles="admin")]
            public IActionResult Create()
            {   
                // display blank form to create a new vehicle
                return View();
            }

            // POST /vehicle/create
            [HttpPost]
            [ValidateAntiForgeryToken]
            [Authorize(Roles="admin")]
            public IActionResult Create([Bind("Make,Model,Year,Registration,FuelType,BodyType,TransmissionType,CC,No0fDoors,MotDue")]  Vehicle v)
            {
                // check registartion is unique for this vehicle
                if (svc.IsDuplicateVehicleReg(v.Registration, v.Id))
                {
                    // add manual validation error
                    ModelState.AddModelError(nameof(v.Registration),"The registration number is already in use");
                }

                // complete POST action to add vehicle
                if (ModelState.IsValid)
                {
                    // pass data to service to store 
                    v = svc.AddVehicle(v.Make, v.Model,v.Year,v.Registration,v.FuelType,v.BodyType,v.TransmissionType,v.CC,v.No0fDoors,v.MotDue);
                    Alert($"New vehicle created successfully", AlertType.success);

                    return RedirectToAction(nameof(Details), new { Id = v.Id});
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
            public IActionResult Edit(int id, [Bind("Make,Model,Year,Registration,FuelType,BodyType,TransmissionType,CC,No0fDoors,MotDue")] Vehicle v)
            {
                // check registration number is unique for this student  
                if (svc.IsDuplicateVehicleReg(v.Registration,v.Id)) {
                    // add manual validation error
                    ModelState.AddModelError("Registration", "This registration number is already registered on the system");
                }

                // validate and complete POST action to save vehicle changes
                if (ModelState.IsValid)
                {
                    // pass data to service to update
                    svc.UpdateVehicle(v);      
                    Alert("Vehicle updated successfully", AlertType.info);

                    return RedirectToAction(nameof(Details), new { Id = v.Id });
                }

                // redisplay the form for editing as validation errors
                return View(v);
            }

            // GET / vehicle/delete/{id}
            [Authorize(Roles="admin")]      
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
            [Authorize(Roles="admin")]
            [ValidateAntiForgeryToken]              
            public IActionResult DeleteConfirm(int id)
            {
                // delete vehicle via service
                svc.DeleteVehicle(id);

                Alert("Vehicle deleted successfully", AlertType.info);
                // redirect to the index view
                return RedirectToAction(nameof(Index));
            }


            // ============== Student ticket management ==============

            // GET /student/createticket/{id}
            public IActionResult TicketCreate(int id)
            {     
                var s = svc.GetStudent(id);
                // check the returned student is not null and if so alert
                if (s == null)
                {
                    Alert($"Student {id} not found", AlertType.warning);
                    return RedirectToAction(nameof(Index));
                }

                // create a ticket view model and set StudentId (foreign key)
                var ticket = new Ticket { StudentId = id }; 

                return View( ticket );
            }

            // POST /student/create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult TicketCreate([Bind("StudentId, Issue")] Ticket t)
            {
                if (ModelState.IsValid)
                {                
                    var ticket = svc.CreateTicket(t.StudentId, t.Issue);
                    Alert($"Ticket created successfully for student {t.StudentId}", AlertType.info);
                    return RedirectToAction(nameof(Details), new { Id = ticket.StudentId });
                }
                // redisplay the form for editing
                return View(t);
            }

            // GET /student/ticketdelete/{id}
            public IActionResult TicketDelete(int id)
            {
                // load the ticket using the service
                var ticket = svc.GetTicket(id);
                // check the returned Ticket is not null and if so return NotFound()
                if (ticket == null)
                {
                    Alert($"Ticket {id} not found", AlertType.warning);
                    return RedirectToAction(nameof(Index));
                }     
                
                // pass ticket to view for deletion confirmation
                return View(ticket);
            }

            // POST /student/ticketdeleteconfirm/{id}
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult TicketDeleteConfirm(int id, int studentId)
            {
                // delete student via service
                svc.DeleteTicket(id);
                Alert($"Ticket deleted successfully for student {studentId}", AlertType.info);
                
                // redirect to the ticket index view
                return RedirectToAction(nameof(Details), new { Id = studentId });
            }

        }
    }