using System;
using System.Text;
using System.Collections.Generic;
using FMS.Data.Models;

namespace FMS.Data.Services
{
    public static class RehomingServiceSeeder
    {
        // use this class to seed the database with dummy test data using an IFleetService
        public static void Seed(IRehomingService svc)
        {
            //wipe and recreate the database
            svc.Initialise();

            //========== Add seed data ==========

            //add dogs
            var d1 = svc.AddDog("Cocker Spaniel","Freddie","LG67 PUT",DateTime.Parse("2022-05-02"),
                                     "https://www.pdsa.org.uk/media/8264/cocker-spaniel-outdoors-gallery-1-min.jpg?anchor=center&mode=crop&quality=100&height=500&bgcolor=fff&rnd=132204646460000000");
            var d2 = svc.AddDog("Sprocker Spaniel","Margo","PG56 BTQ",DateTime.Parse("2021-11-25"),
                                     "");
            var d3 = svc.AddDog("Shih Tzu","Poppie","MQ12 YIS",DateTime.Parse("2020-06-13"),
                                    "https://patterjack.com/wp-content/uploads/2021/11/shih_tzu_article_c.jpg");
            var d4 = svc.AddDog("Beagle","Georgie","LM41 GTL",DateTime.Parse("2018-08-09"),
                                    "https://th.bing.com/th/id/R.53abca2d5c19b770a28205de25977876?rik=95Gcy31wNrWN4Q&riu=http%3a%2f%2fwww.petguide.com%2fwp-content%2fuploads%2f2013%2f02%2fbeagle-1.jpg&ehk=O5TbIG8SdlQUhyXhQaWVMkuQG4NXzpzBd6tcvIcj8xw%3d&risl=&pid=ImgRaw&r=0");
           

            //add medical history for dog 1
            var medicalHistory1 = svc.CreateMedicalHistory(d1.Id,DateTime.Parse("2021-05-15"),"Euan Barlow","Cured","Broken leg");
            var medicalHistory2 = svc.CreateMedicalHistory(d1.Id,DateTime.Parse("2020-05-15"),"Euan Barlow","Ongoing","Arthritis");
            var medicalHistory3 = svc.CreateMedicalHistory(d1.Id,DateTime.Parse("2019-05-15"),"Euan Barlow","Cured","Surgery on heart"); 

            //add medical history for dog 2
            var mot4 = svc.CreateMedicalHistory(d2.Id,DateTime.Parse("2021-05-15"),"Euan Barlow","Cured","Got neutered, required stitches");

            //add medical history for dog 3
            var mot5 = svc.CreateMedicalHistory(d3.Id,DateTime.Parse("2021-05-15"),"Adam Crozier","Cured","Broken leg");

            //add users
            var u1 = svc.Register("Guest", "guest@sms.com", "guest", Role.guest);
            var u2 = svc.Register("Administrator", "admin@sms.com", "admin", Role.admin);
            var u3 = svc.Register("Manager", "manager@sms.com", "manager", Role.manager);

            

        }
    }
}
