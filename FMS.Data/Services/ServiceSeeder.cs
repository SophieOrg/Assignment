using System;
using System.Text;
using System.Collections.Generic;
using FMS.Data.Models;

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
            var datev1 = new DateOnly(2022,05,15);
            var datev2 = new DateOnly(2022,07,10);
            var datev3 = new DateOnly(2022,06,13);
            var datev4 = new DateOnly(2022,08,09);

            //add vehicles
            var v1 = svc.AddVehicle("VW","Golf",2018,"LG67 PUT","Diesel","Hatchback","Manual",1600,3,datev1,"https://m.atcdn.co.uk/a/media/w540/0f4ebd9ffd4b4ed197a96174d60af4f1.jpg");
            var v2 = svc.AddVehicle("Porsche","Cayenne",2018,"PG56 BTQ","Diesel","Coupe","Automatic",2400,5,datev2,"https://www.kindpng.com/picc/m/162-1629507_porsche-cayenne-gts-car-hire-front-view-car.png");
            var v3 = svc.AddVehicle("Maserati","Quattroporte",2019,"MQ12 YIS","Petrol","Hatchback","Automatic",2000,5,datev3,"https://imagecdn.leasingoptions.co.uk/fit-in/570x380/image/vehicles/ids/maserati/quattroporte/quattroporte-saloon/v8-trofeo-4dr-auto/94914/front_view/b504a1b0-4f5b-4927-a1bc-08ed145b3d2b.jpg");
            var v4 = svc.AddVehicle("Bentley","Bentayga",2018,"LM41 GTL","Diesel","Hatchback","Automatic",1800,5,datev4,"https://www.seekpng.com/png/detail/836-8367072_2018-bentley-bentayga-black-edition-2018-bentley-bentayga.png");
           

            //add mot history for vehicle 1

            var motdate1 = new DateOnly(2021,05,15);
            var motdate2 = new DateOnly(2020,05,15);
            var motdate3 = new DateOnly(2019,05,15);
            var motdate4 = new DateOnly(2021,05,15);
            var motdate5 = new DateOnly(2021,05,15);

            var mot1 = svc.CreateMot(v1.Id,motdate1,"Simon Cowan","Fail",34000, "Minor fault with wiper");
            var mot2 = svc.CreateMot(v1.Id,motdate2,"Craig Getty","Fail",68000, "Front right headlight not working");
            var mot3 = svc.CreateMot(v1.Id,motdate3,"Euan Barlow","Pass",90000, "Low tyre pressure"); 

            //add mot history for vehicle 2
            var mot4 = svc.CreateMot(v2.Id,motdate4,"Andrew Johnston","Pass",40000, "Headlights out");

            //add mot history for vehicle 3
            var mot5 = svc.CreateMot(v3.Id,motdate5,"Adam Trouton","Pass",56000, "Need new timing belt");

            //add users
            var u1 = svc.Register("Guest", "guest@sms.com", "guest", Role.guest);
            var u2 = svc.Register("Administrator", "admin@sms.com", "admin", Role.admin);
            var u3 = svc.Register("Manager", "manager@sms.com", "manager", Role.manager);

            

        }
    }
}
