using System;
using Xunit;
using FMS.Data.Models;
using FMS.Data.Services;

namespace FMS.Test
{

    public class ServiceTests
    {
        private readonly IFleetService svc;


        public ServiceTests()
        {
            // general arrangement
            svc = new FleetServiceDb();
          
            // ensure data source is empty before each test
            svc.Initialise();
        }

        // ========================== Vehicle Fleet Tests =========================
        [Fact] 
        public void Vehicle_AddVehicle_WhenDuplicateReg_ShouldReturnNull()
        {
            // act  
            var v1 = svc.AddVehicle("Bentley","Bentayga",2018,"LM41 GTL","Diesel","Hatchback","Automatic",1800,5,DateTime.Parse("2022-05-06"),"");
            // this is a duplicate as the registration is same as previous vehicle
            var v2 = svc.AddVehicle("Bentley","Bentayga",2018,"LM41 GTL","Diesel","Hatchback","Automatic",1800,5,DateTime.Parse("2022-05-06"),"");
            
            // assert
            Assert.NotNull(v1); // this vehicle should have been added correctly(if test returns notnull, vehicle added because car not found with that reg)
            Assert.Null(v2); // this vehicle should NOT have been added        
        }

        [Fact]
        public void Vehicle_UpdateVehicle_ThatExists_ShouldSetAllProperties()
        {
            // arrange - create test vehicle
            var v = svc.AddVehicle("Volkswagon", "Golf",2017,"AFZ4440","Petrol","Hatchback","Fumes",2000,5,DateTime.Parse("2010-05-08"),"");
                        
            // act - create a copy and update any vehicle properties (except Id) 
            var u = new Vehicle
            {
                Id = v.Id,
                Make = "Volkswagon",
                Model = "Golf",
                Year = 2017,
                Registration = "AFZ4440",
                FuelType = "Petrol",
                BodyType = "Hatchback",
                TransmissionType = "Automatic",
                CC = 2000,
                No0fDoors = 5,
                MotDue = DateTime.Parse("2010-05-08"),
                PhotoUrl = ""
            };
            // save updated vehicle
            svc.UpdateVehicle(u); 

            // reload updated vehicle from database into vu
            var vu = svc.GetVehicle(v.Id);

            // assert
            Assert.NotNull(u);           

            // now assert that the properties were set properly           
            Assert.Equal(u.Make, vu.Make);
            Assert.Equal(u.Model, vu.Model);
            Assert.Equal(u.Year, vu.Year);
            Assert.Equal(u.Registration, vu.Registration);
            Assert.Equal(u.FuelType, vu.FuelType);
            Assert.Equal(u.BodyType, vu.BodyType);
            Assert.Equal(u.TransmissionType, vu.TransmissionType);
            Assert.Equal(u.FuelType, vu.FuelType);
            Assert.Equal(u.CC, vu.CC);
            Assert.Equal(u.No0fDoors, vu.No0fDoors);
            Assert.Equal(u.MotDue, vu.MotDue);
            Assert.Equal(u.PhotoUrl, vu.PhotoUrl);
            
        }

        [Fact] 
        public void Vehicle_GetAllVehicles_WhenNone_ShouldReturn0()
        {   
            //There is no arrange step here as we are not adding any vehicles to the database
            // act 
            var vehicles = svc.GetVehicles();
            var count = vehicles.Count;

            // assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void Vehicle_GetVehicles_When3Exist_ShouldReturn2()
        {
            // arrange - add 3 different vehicles to database
            var v1 = svc.AddVehicle("Volkswagon", "Golf",2017,"AFZ4440","Petrol","Hatchback","Manual",2000,5,DateTime.Parse("2022-05-08"),"");
            var v2 = svc.AddVehicle("Ford", "Fiesta",2013,"BJZ4340","Diesel","Hatchback","Automatic",2000,5,DateTime.Parse("2022-05-08"),"");
            var v3 = svc.AddVehicle("Renault", "Megane",2017,"GHX3340","Petrol","Hatchback","Automatic",1800,3,DateTime.Parse("2022-05-08"),"");
            // act
            var vehicles = svc.GetVehicles();
            var count = vehicles.Count;

            // assert
            Assert.Equal(3, count);
        }

         [Fact]
        public void Vehicle_DeleteVehicle_ThatExists_ShouldReturnTrue()
        {
            // act - add the vehicle to the database then delete the vehicle from the database
            var v1 = svc.AddVehicle("Volkswagon", "Golf",2017,"AFZ4440","Petrol","Hatchback","Manual",2000,5,DateTime.Parse("2022-05-08"),"");
            var deleted = svc.DeleteVehicle(v1.Id);

            // try to retrieve deleted vehicle
            var v = svc.GetVehicle(v1.Id);

            // assert
            Assert.True(deleted); // delete vehicle should return true
            Assert.Null(v);      // v should be null as the vehicle cannot be retrieved
        }

         [Fact]
        public void Vehicle_DeleteVehicle_ThatDoesntExist_ShouldReturnFalse()
        {
            // act 	
            var deleted = svc.DeleteVehicle(0);

            // assert
            Assert.False(deleted);
        }  

         [Fact]
        public void Vehicle_UpdateVehicle_ThatExistsWithNewTransmissionType_ShouldWork()
        {
            // arrange
            var v1 = svc.AddVehicle("Volkswagon", "Golf",2017,"AFZ4440","Petrol","Hatchback","Manual",2000,3,DateTime.Parse("2022-05-08"),"");

            // act
            // create a copy of added vehicle and change trasnmission type to automatic
            var v = new Vehicle 
            {   
                Id = v1.Id,
                Make = v1.Make,
                Model = v1.Model,
                Year = v1.Year,
                Registration = v1.Registration,
                FuelType = v1.FuelType,
                BodyType = v1.BodyType,
                TransmissionType = v1.TransmissionType = "Automatic", 
                CC = v1.CC,
                No0fDoors = v1.No0fDoors,
                MotDue = v1.MotDue,
                PhotoUrl = v1.PhotoUrl              
            };
            // update this vehicle
            svc.UpdateVehicle(v);

            // now load the vehicle and verify tarsnmission type was updated
            var su = svc.GetVehicle(v.Id);

            // assert
            Assert.Equal(v.TransmissionType, su.TransmissionType);
        }

        // =============== MOT Ticket Tests ================
        
        [Fact] 
        public void Ticket_CreateMotTicket_ForExistingVehicle_ShouldBeCreated()
        {
            // arrange
            var v1 = svc.AddVehicle("Volkswagon", "Golf",2017,"AFZ4440","Petrol","Hatchback","Manual",2000,3,DateTime.Parse("2022-05-08"),"");
         
            // act
            var t = svc.CreateMot(v1.Id, DateTime.Parse("2022-06-15"),"Margaret Crozier","Pass",38000,"Top up oil");
           
            // assert
            Assert.NotNull(t); //t should return "not null" because an MOT ticket has been successfully created
            Assert.Equal(v1.Id, t.VehicleId); 
        }
       
       
        [Fact] 
        public void Ticket_DeleteMotTicket_WhenExists_ShouldReturnTrue()
        {
            // arrange
            var v1 = svc.AddVehicle("Volkswagon", "Golf",2017,"AFZ4440","Petrol","Hatchback","Manual",2000,3,DateTime.Parse("2022-05-08"),"");
            var t = svc.CreateMot(v1.Id, DateTime.Parse("2022-06-15"),"Margaret Crozier","Pass",38000,"Top up oil");

            // act
            var deleted = svc.DeleteMotTicket(t.Id);     // delete ticket    
            
            // assert
            Assert.True(deleted);                    // ticket should be deleted
        }    

        
        // =================  User Tests ===========================
        
        [Fact] // --- Register Valid User test
        public void User_Register_WhenValid_ShouldReturnUser()
        {
            // arrange 
            var reg = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
            
            // act
            var user = svc.GetUserByEmail(reg.Email);
            
            // assert
            Assert.NotNull(reg);
            Assert.NotNull(user);
        } 

        [Fact] // --- Register Duplicate Test
        public void User_Register_WhenDuplicateEmail_ShouldReturnNull()
        {
            // arrange 
            var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
            
            // act
            var s2 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);

            // assert
            Assert.NotNull(s1);
            Assert.Null(s2);
        } 

        [Fact] // --- Authenticate Invalid Test
        public void User_Authenticate_WhenInValidCredentials_ShouldReturnNull()
        {
            // arrange 
            var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
        
            // act
            var user = svc.Authenticate("xxx@email.com", "guest");
            // assert
            Assert.Null(user);

        } 

        [Fact] // --- Authenticate Valid Test
        public void User_Authenticate_WhenValidCredentials_ShouldReturnUser()
        {
            // arrange 
            var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
        
            // act
            var user = svc.Authenticate("xxx@email.com", "admin");
            
            // assert
            Assert.NotNull(user);
        } 



    }
}