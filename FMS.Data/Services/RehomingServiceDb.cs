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
        
        //Add a new dog checking chip number is unique
        public Dog AddDog(string breed,string name,string chipNumber,DateTime dob, string photoUrl)
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
                DOB = dob,
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
            dog.DOB = updated.DOB;
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
        public MedicalHistory CreateMedicalHistory(int dogId,DateTime on, string vet,string status, string report)
        {   
            var dog = GetDog(dogId);
            if(dog == null) return null;

            var medicalHistory = new MedicalHistory
            {
                //Id created by database
                DogId = dogId,
                On = on,
                Vet = vet,
                Status = status,
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
            var mot = GetMedicalHistory(id);
            if(DeleteMedicalHistoryNote == null) return false;

            //remove Medical History Note
            var result = db.MedicalHistorys.Remove(mot);

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
