using System;
using System.Text;
using System.Collections.Generic;

namespace FMS.Data.Services
{
    public static class FleetServiceSeeder
    {
        // use this class to seed the database with dummy test data using an IFleetService
        public static void Seed(IFleetService svc)
        {
            //wipe and recreate the database
            svc.Initialise();

            //========== Add seed data ==========

            //add vehicles
            var v1 = svc.AddVehicle("VW","Golf",2018,"LG67 PUT","Diesel","Hatchback","Manual",1600,3,);
            var v2 = svc.AddVehicle("Porsche","Cayenne",2018,"PG56 BTQ","Diesel","Coupe","Automatic",2400,5,2021,05,20);
            var v3 = svc.AddVehicle("Maserati","Quattroporte",2019,"MQ12 YIS","Petrol","Hatchback","Automatic",2000,5,2021,02,11);
            var v4 = svc.AddVehicle("Bentley","Bentayga",2018,"LM41 GTL","Diesel","Hatchback","Automatic",1800,5,2020,06,05);
           

            //add mot history for vehicle 1
            var mot1 = svc.CreateMot(v1.Id, "Minor fault with wiper");
            var mot2 = svc.CreateMot(v1.Id, "Front right headlight not working");
            var mot3 = svc.CreateMot(v1.Id, "Low tyre pressure"); 

            //add mot history for vehicle 2
            var mot4 = svc.CreateMot(v2.Id, "Headlights out");

            //add mot history for vehicle 3
            var mot5 = svc.CreateMot(v3.Id, "Need new timing belt");

            //close vehicle 1's first mot history
             svc.CloseMot(v1.Id, "Wiper fixed");

            //add users
            var u1 = svc.Register("Guest", "guest@sms.com", "guest", Role.guest);
            var u2 = svc.Register("Administrator", "admin@sms.com", "admin", Role.admin);
            var u3 = svc.Register("Manager", "manager@sms.com", "manager", Role.manager);

            

        }
    }
}
