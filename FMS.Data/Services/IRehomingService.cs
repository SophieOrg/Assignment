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

        public Dog GetDogAndApplications(int id);

        Dog GetDogByChipNumber(string chipNumber);

        Dog AddDog(string breed,string name,string chipNumber,int age,string Information, string photoUrl);

        Dog UpdateDog(Dog updated);

        bool DeleteDog(int id);

        bool IsDuplicateDogChipped(string chipNumber, int dogId);        
    

        // ------------- User Management -------------------
        User Authenticate(string email, string password);
        User Register(string name, string email, string password, Role role);
        User GetUserByEmail(string email);

        // ------------- Medical History Management -------------------

        MedicalHistory CreateMedicalHistory(int dogId,string medication, string report);
        MedicalHistory GetMedicalHistory(int id);
        bool DeleteMedicalHistoryNote(int id);
        MedicalHistory CloseMedicalHistoryNote(int id, string resolution);

        IList<MedicalHistory> GetAllMedicalHistoryNotes();
        IList<MedicalHistory> GetOpenMedicalHistoryNotes();        
        IList<MedicalHistory> SearchMedicalHistoryNotes(TicketRange range, string query);

        // ------------- Adoption Application Management -------------------

        AdoptionApplication CreateAdoptionApplication(int dogId,string name, string email, string phoneNumber, string information);
        AdoptionApplication GetAdoptionApplication(int id);
        bool DeleteAdoptionApplication(int id);

        IList<AdoptionApplication> GetAllAdoptionApplications();
        IList<AdoptionApplication> SearchAdoptionApplications(AdoptionApplicationRange range, string query);

        IList<AdoptionApplication> GetValidAdoptionApplications();

        AdoptionApplication ApproveAdoptionApplication(int id, string resolution);

    }
    
}