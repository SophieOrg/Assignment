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

        // ========================== Fleet Tests =========================
        [Fact] 
        public void Vehicle_AddVehicle_WhenDuplicateReg_ShouldReturnNull()
        {
            // act 
            var datev1 = new DateTime(2022,05,06);
            
            var v1 = svc.AddVehicle("Bentley","Bentayga",2018,"LM41 GTL","Diesel","Hatchback","Automatic",1800,5,datev1,"");
            // this is a duplicate as the registration is same as previous vehicle
            var v2 = svc.AddVehicle("Bentley","Bentayga",2018,"LM41 GTL","Diesel","Hatchback","Automatic",1800,5,datev1,"");
            
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
                TransmissionType = "Fumes",
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

    }
}