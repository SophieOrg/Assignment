using System;
using System.Collections.Generic;
	
using FMS.Data.Models;
	
namespace FMS.Data.Services
{
    // This interface describes the operations that a FleetService class should implement
    public interface IRehomingService
    {
        //Initialise the repository (database)
        void Initialise();

        // ------------- Dog Management -------------------   
        IList<Dog> GetDogs(); 

        Dog GetDog(int id);

        Dog GetDogByChipNumber(string chipNumber);

        Dog AddDog(string breed,string name,string chipNumber,DateTime dob, string photoUrl);

        Dog UpdateDog(Dog updated);

        bool DeleteDog(int id);

        bool IsDuplicateDogChipped(string chipNumber, int dogId);        
    

        // ------------- User Management -------------------
        User Authenticate(string email, string password);
        User Register(string name, string email, string password, Role role);
        User GetUserByEmail(string email);

        // ------------- Medical History Management -------------------

        MedicalHistory CreateMedicalHistory(int vehicleId,DateTime on, string vet,string status,string report);
        MedicalHistory GetMedicalHistory(int id);
        bool DeleteMedicalHistoryNote(int id);
        MedicalHistory CloseTicket(int id, string resolution);

        IList<MedicalHistory> GetAllTickets();
        IList<MedicalHistory> GetOpenTickets();        
        IList<MedicalHistory> SearchTickets(TicketRange range, string query);
    
    }
    
}