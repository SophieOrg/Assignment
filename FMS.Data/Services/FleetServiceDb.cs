using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using FMS.Data.Models;
using FMS.Data.Repository;
using FMS.Data.Security;

namespace FMS.Data.Services
{
    public class FleetServiceDb : IFleetService
    {
        private readonly DataContext db;

        public FleetServiceDb()
        {
            db = new DataContext();
        }

        public void Initialise()
        {
            db.Initialise(); // recreate database
        }

        // ==================== Fleet Management ==================
       
        // Implementing IFleetService methods here
        
        // ==================== Vehicle Related Operations ==================

        //retrieve list of vehicles
        public IList<Vehicle> GetVehicles()
        {
            return db.Vehicles.ToList();
        }
        
        //Retrieve vehicle by Id and related Mot tickets
        public Vehicle GetVehicle(int id)
        {
            return db.Vehicles
                     .Include(v => v.Mots)
                     .FirstOrDefault(v => v.Id == id);
        }
        
        //Add a new vehicle checking registration is unique
        public Vehicle AddVehicle(string make,string model,int year,string registration,string fuelType,string bodyType,string transmissionType,int cc, int no0fDoors, DateTime motDue, string photoUrl)
        {
            //check if vehicle with registration exists
            var exists = GetVehicleByRegistration(registration);
            if (exists != null)
            {       
                return null;
            }

            //create a new vehicle
            var v = new Vehicle
            {
                Make = make,
                Model = model,
                Year = year,
                Registration = registration,
                FuelType = fuelType,
                BodyType = bodyType,
                TransmissionType = transmissionType,
                CC = cc,
                No0fDoors = no0fDoors,
                MotDue = motDue,
                PhotoUrl =photoUrl
            };
            
            //Add Vehicle to the list
            db.Vehicles.Add(v);

            //Save the changes
            db.SaveChanges();

            //return newly added vehicle
            return v;
        }
        
        //Delete the vehicle identified by Id returning true if deleted
        //and false if not found
        public bool DeleteVehicle(int id)
        {
            var v = GetVehicle(id);
            if (v == null)
            {
                return false;
            }

            db.Vehicles.Remove(v);
            db.SaveChanges();
            return true;
        }
        
        //Update the Vehicle with the details in updated
        public Vehicle UpdateVehicle(Vehicle updated)
        {
            //verify the vehicle exists
            var vehicle = GetVehicle(updated.Id);
            if (vehicle == null)
            {
                return null;
            }

            //update the details of the vehicle retrieved and save
            vehicle.Make = updated.Make;
            vehicle.Model = updated.Model;
            vehicle.Year = updated.Year;
            vehicle.Registration = updated.Registration;
            vehicle.FuelType = updated.FuelType;
            vehicle.BodyType = updated.BodyType;
            vehicle.TransmissionType = updated.TransmissionType;
            vehicle.CC = updated.CC;
            vehicle.No0fDoors = updated.No0fDoors;
            vehicle.MotDue = updated.MotDue;
            vehicle.PhotoUrl = updated.PhotoUrl;

            db.SaveChanges();
            return vehicle;

        }


        public Vehicle GetVehicleByRegistration(string registration)
        {
            return db.Vehicles.FirstOrDefault(v => v.Registration == registration);
        }


        public bool IsDuplicateVehicleReg(string registration, int vehicleId)
        {
            var existing = GetVehicleByRegistration(registration);

            //if a vehicle with a registration exists and the Id does not match the 
            //vehicleId (if provided), then they can't use that registration
            return existing != null && vehicleId != existing.Id;
        }    

         
        // ==================== Mot Ticket Management ==================
        public Mot CreateMot(int vehicleId,DateTime on, string motTester,string status,int mileage, string report)
        {   
            var vehicle = GetVehicle(vehicleId);
            if(vehicle == null) return null;

            var mot = new Mot
            {
                //Id created by database
                VehicleId = vehicleId,
                On = on,
                MotTester = motTester,
                Status = status,
                Mileage = mileage,
                Report = report,

            };
            db.Mots.Add(mot);
            db.SaveChanges();
            return mot;

        }

        public Mot GetMot(int id)
        {   
            //return ticket and related vehicle or null if not found
            return db.Mots
                    .Include(v => v.Vehicle)
                    .FirstOrDefault(v => v.Id == id);

        }


        public bool DeleteMotTicket(int id)
        {   
            //find Mot ticket
            var mot = GetMot(id);
            if(DeleteMotTicket == null) return false;

            //remove ticket
            var result = db.Mots.Remove(mot);

            db.SaveChanges();
            return true;

        }


        // ==================== User Authentication/Registration Management ==================
        public User Authenticate(string email, string password)
        {
            // retrieve the user based on the EmailAddress (assumes EmailAddress is unique)
            var user = GetUserByEmail(email);

            // Verify the user exists and Hashed User password matches the password provided
            return (user != null && Hasher.ValidateHash(user.Password, password)) ? user : null;
        }

        public User Register(string name, string email, string password, Role role)
        {
            // check that the user does not already exist (unique user name)
            var exists = GetUserByEmail(email);
            if (exists != null)
            {
                return null;
            }

            // Custom Hasher used to encrypt the password before storing in database
            var user = new User 
            {
                Name = name,
                Email = email,
                Password = Hasher.CalculateHash(password),
                Role = role   
            };
   
            db.Users.Add(user);
            db.SaveChanges();
            return user;
        }

        public User GetUserByEmail(string email)
        {
            return db.Users.FirstOrDefault(u => u.Email == email);
        }

    }
}
