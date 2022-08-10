using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using FMS.Data.Models;
using FMS.Data.Repository;
using FMS.Data.Security;

namespace FMS.Data.Services
{
    public class RehomingServiceDb : IRehomingService
    {
        private readonly DataContext db;

        public RehomingServiceDb()
        {
            db = new DataContext();
        }

        public void Initialise()
        {
            db.Initialise(); // recreate database
        }

        // ==================== Rehoming Centre Management ==================
       
        // Implementing IRehomingService methods here
        
        // ==================== Dog Related Operations ==================

        //retrieve list of dogs
        public IList<Dog> GetDogs()
        {
            return db.Dogs.ToList();
        }
        
        //Retrieve Dog by Id and related Medical History notes
        public Dog GetDog(int id)
        {
            return db.Dogs
                     .Include(v => v.MedicalHistorys)
                     .FirstOrDefault(v => v.Id == id);
        }

        //Retrieve Dog by Id and related adoption applications
        public Dog GetDogAndApplications(int id)
        {   
            return db.Dogs
                     .Include(v => v.AdoptionApplications)
                     .FirstOrDefault(v => v.Id == id);

        }
        
        //Add a new dog checking chip number is unique
        public Dog AddDog(string breed,string name,string chipNumber,int age, string information, string photoUrl)
        {
            //check if dog with chip number exists
            var exists = GetDogByChipNumber(chipNumber);
            if (exists != null)
            {       
                return null;
            }

            //create a new dog
            var v = new Dog
            {
                Breed = breed,
                Name = name,
                ChipNumber = chipNumber,
                Age = age,
                Information = information,
                PhotoUrl =photoUrl
            };
            
            //Add Dog to the list
            db.Dogs.Add(v);

            //Save the changes
            db.SaveChanges();

            //return newly added dog
            return v;
        }
        
        //Delete the dog identified by Id returning true if deleted
        //and false if not found
        public bool DeleteDog(int id)
        {
            var v = GetDog(id);
            if (v == null)
            {
                return false;
            }

            db.Dogs.Remove(v);
            db.SaveChanges();
            return true;
        }
        
        //Update the Dogs with the details in updated
        public Dog UpdateDog(Dog updated)
        {
            //verify the dog exists
            var dog = GetDog(updated.Id);
            if (dog == null)
            {
                return null;
            }

            //update the details of the dog retrieved and save
            dog.Breed = updated.Breed;
            dog.Name = updated.Name;
            dog.ChipNumber = updated.ChipNumber;
            dog.Age = updated.Age;
            dog.Information = updated.Information;
            dog.PhotoUrl = updated.PhotoUrl;

            db.SaveChanges();
            return dog;

        }


        public Dog GetDogByChipNumber(string chipNumber)
        {
            return db.Dogs.FirstOrDefault(v => v.ChipNumber == chipNumber);
        }


        public bool IsDuplicateDogChipped(string chipNumber, int dogId)
        {
            var existing = GetDogByChipNumber(chipNumber);

            //if a dog with a chip number exists and the Id does not match the 
            //dogId (if provided), then they can't use that chip number
            return existing != null && dogId != existing.Id;
        }    

         
        // ==================== Medical History Note Management ==================
        public MedicalHistory CreateMedicalHistory(int dogId,string medication, string report)
        {   
            var dog = GetDog(dogId);
            if(dog == null) return null;

            var medicalHistory = new MedicalHistory
            {
                //Id created by database
                DogId = dogId,
                CreatedOn = DateTime.Now,
                Active = true,
                Medication = medication,
                Report = report,

            };
            db.MedicalHistorys.Add(medicalHistory);
            db.SaveChanges();
            return medicalHistory;

        }

        public MedicalHistory GetMedicalHistory(int id)
        {   
            //return medical history note and related dog or null if not found
            return db.MedicalHistorys
                    .Include(v => v.Dog)
                    .FirstOrDefault(v => v.Id == id);

        }


        public bool DeleteMedicalHistoryNote(int id)
        {   
            //find Medical History Note
            var medNote = GetMedicalHistory(id);
            if(DeleteMedicalHistoryNote == null) return false;

            //remove Medical History Note
            var result = db.MedicalHistorys.Remove(medNote);

            db.SaveChanges();
            return true;

        }

         // Retrieve all tickets and the student associated with the ticket
        public IList<MedicalHistory> GetAllMedicalHistoryNotes()
        {
            return db.MedicalHistorys
                     .Include(t => t.Dog)
                     .ToList();
        }

        // Retrieve all open tickets (Active)
        public IList<MedicalHistory> GetOpenMedicalHistoryNotes()
        {
            // return open tickets with associated students
            return db.MedicalHistorys
                     .Include(t => t.Dog) 
                     .Where(t => t.Active)
                     .ToList();
        } 

        // perform a search of the tickets based on a query and
        // an active range 'ALL', 'OPEN', 'CLOSED'
        public IList<MedicalHistory> SearchMedicalHistoryNotes(TicketRange range, string query) 
        {
            // ensure query is not null    
            query = query == null ? "" : query.ToLower();

            // search ticket issue, active status and student name
            var results = db.MedicalHistorys
                            .Include(t => t.Dog)
                            .Where(t => (t.Report.ToLower().Contains(query) || 
                                         t.Dog.Name.ToLower().Contains(query)
                                        ) &&
                                        (range == TicketRange.OPEN && t.Active ||
                                         range == TicketRange.CLOSED && !t.Active ||
                                         range == TicketRange.ALL
                                        ) 
                            ).ToList();
            return  results;  
        }

         public MedicalHistory CloseMedicalHistoryNote(int id, string resolution)
        {
            var ticket = GetMedicalHistory(id);
            // if ticket does not exist or is already closed return null
            if (ticket == null || !ticket.Active) return null;
            
            // ticket exists and is active so close
            ticket.Active = false;
            ticket.Resolution = resolution;
            ticket.ResolvedOn = DateTime.Now;
           
            db.SaveChanges(); // write to database
            return ticket;
        }

        // ===============Adoption application management ========================
        public AdoptionApplication CreateAdoptionApplication(int dogId,string name, string email, string phoneNumber, string information)
        {   
            var dog = GetDogAndApplications(dogId);
            if(dog == null) return null;

            var adoptionApplication = new AdoptionApplication
            {
                //Id created by database
                DogId = dogId,
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Information = information,
            };

            db.AdoptionApplications.Add(adoptionApplication);
            db.SaveChanges();
            return adoptionApplication;

        }

        public AdoptionApplication GetAdoptionApplication(int id)
        {
            return db.AdoptionApplications
                     .Include(v => v.Dog)
                     .FirstOrDefault(v => v.Id == id);
        }

        public bool DeleteAdoptionApplication(int id)
        {
            var adoptionApplication = GetAdoptionApplication(id);
            if(DeleteAdoptionApplication == null) return false;

            var result = db.AdoptionApplications.Remove(adoptionApplication);

            db.SaveChanges();
            return true;
        }

        public IList <AdoptionApplication> GetAllAdoptionApplications()
        {
            return db.AdoptionApplications
                     .Include(t => t.Dog)
                     .ToList();
        }
        
                
        // Retrieve all open tickets (Active)
        public IList<AdoptionApplication> GetValidAdoptionApplications()
        {
            // return valid applications with associated dogs
            return db.AdoptionApplications
                     .Include(t => t.Dog) 
                     .Where(t => t.Active)
                     .ToList();
        } 

        // perform a search of the tickets based on a query and
        // an active range 'ALL', 'VALID', 'INVALID'
        public IList<AdoptionApplication> SearchAdoptionApplications(AdoptionApplicationRange range, string query) 
        {
            // ensure query is not null    
            query = query == null ? "" : query.ToLower();

            // search active status and student name
            var results = db.AdoptionApplications
                            .Include(t => t.Dog)
                            .Where(t => (
                                         t.Dog.Name.ToLower().Contains(query)
                                        ) &&
                                        (range == AdoptionApplicationRange.AWAITING && t.Active ||
                                         range == AdoptionApplicationRange.APPROVED && !t.Active ||
                                         range == AdoptionApplicationRange.ALL
                                        ) 
                            ).ToList();
            return  results;  
        }

         public AdoptionApplication ApproveAdoptionApplication(int id, string resolution)
        {
            var adoptionapplication = GetAdoptionApplication(id);
            // if ticket does not exist or is already closed return null
            if (adoptionapplication == null || !adoptionapplication.Active)
            {
                return null;
            } 
            
            // ticket exists and is active so close
            adoptionapplication.Active = false;
            adoptionapplication.Resolution = resolution;
           
            db.SaveChanges(); // write to database
            
            return adoptionapplication;
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
