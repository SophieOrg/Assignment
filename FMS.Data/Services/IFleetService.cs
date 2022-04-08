using System;
using System.Collections.Generic;
	
using FMS.Data.Models;
	
namespace FMS.Data.Services
{
    // This interface describes the operations that a FleetService class should implement
    public interface IFleetService
    {
        void Initialise();
        
        // add suitable method definitions to implement assignment requirements

        // ------------- Vehicle Management -------------------   
        IList<Vehicle> GetVehicle(); 

        Vehicle GetVehicle(int id);

        Vehicle GetVehicleByRegistration(string registration);

        Vehicle AddVehicle(string make,string model,int year,string registration,string fuelType,string bodyType,string transmissionType,int cc, int no0fDoors, DateOnly motDue,double price,string photoUrl);

        Vehicle UpdateVehicle(Vehicle updated);

        bool DeleteVehicle(int id);

        bool IsDuplicateVehicleReg(string registration, int vehicleId);        
    

        // ------------- User Management -------------------
        User Authenticate(string email, string password);
        User Register(string name, string email, string password, Role role);
        User GetUserByEmail(string email);

        // ------------- MOT Management -------------------

        Mot CreateMot(int vehicleId, string report);

        Mot GetMot(int id);

        Mot CloseMot(int id,string resolution);

        bool DeleteMotTicket(int id);

        IList<Mot> GetAllMots();

        IList<Mot> GetOpenMots();

        //IList<Mot> SearchMots(MotRange range, string query);
    
    }
    
}