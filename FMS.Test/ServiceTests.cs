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
            var datev1 = new DateOnly(2022,05,06);
            
            var s1 = svc.AddVehicle("Bentley","Bentayga",2018,"LM41 GTL","Diesel","Hatchback","Automatic",1800,5,datev1,"");
            // this is a duplicate as the email address is same as previous student
            var s2 = svc.AddVehicle("Bentley","Bentayga",2018,"LM41 GTL","Diesel","Hatchback","Automatic",1800,5,datev1,"");
            
            // assert
            Assert.NotNull(s1); // this student should have been added correctly
            Assert.Null(s2); // this student should NOT have been added        
        }

        // write suitable tests to verify operation of the fleet service

    }
}