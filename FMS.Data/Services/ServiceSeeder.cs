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
            var d1 = svc.AddDog("Cocker Spaniel","Freddie","145C",DateTime.Parse("2022-05-02"),
                                     "https://www.pdsa.org.uk/media/8264/cocker-spaniel-outdoors-gallery-1-min.jpg?anchor=center&mode=crop&quality=100&height=500&bgcolor=fff&rnd=132204646460000000");
            var d2 = svc.AddDog("Sprocker Spaniel","Margo","863F",DateTime.Parse("2021-11-25"),
                                     "https://sprockerlovers.com/wp-content/uploads/2020/10/9450861e-19e3-43f3-b784-71a368cd8b57-1024x1024.jpeg");
            var d3 = svc.AddDog("Shih Tzu","Poppie","937B",DateTime.Parse("2020-06-13"),
                                    "https://patterjack.com/wp-content/uploads/2021/11/shih_tzu_article_c.jpg");
            var d4 = svc.AddDog("Beagle","Georgie","275H",DateTime.Parse("2018-08-09"),
                                    "https://th.bing.com/th/id/R.53abca2d5c19b770a28205de25977876?rik=95Gcy31wNrWN4Q&riu=http%3a%2f%2fwww.petguide.com%2fwp-content%2fuploads%2f2013%2f02%2fbeagle-1.jpg&ehk=O5TbIG8SdlQUhyXhQaWVMkuQG4NXzpzBd6tcvIcj8xw%3d&risl=&pid=ImgRaw&r=0");
           
            var d5 = svc.AddDog("Schnauzer","Max","469B",DateTime.Parse("2020-08-11"),
                                    "https://media-be.chewy.com/wp-content/uploads/2021/06/24103725/MiniatureSchnauzer-FeaturedImage-1024x615.jpg");

            var d6 = svc.AddDog("Husky","Koda","378D",DateTime.Parse("2021-05-12"),
                                    "https://cdn.britannica.com/84/232784-050-1769B477/Siberian-Husky-dog.jpg");
            var d7 = svc.AddDog("Bichon Frise","Flossy","267H",DateTime.Parse("2021-04-17"),
                                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSyv_t1dDFRw7dCgK3aaE0ap74nuJxqcJKijbJZmtwiVW0aye9cVXtr-jAg5nkUvgY_dic&usqp=CAU");
            var d8 = svc.AddDog("Grey Hound","Finn","244G",DateTime.Parse("2020-04-03"),
                                    "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5d/Italian_Greyhound_standing_gray_%28cropped%29.jpg/1200px-Italian_Greyhound_standing_gray_%28cropped%29.jpg");
            var d9 = svc.AddDog("Bassett Hound","Winston","463E",DateTime.Parse("2018-06-21"),
                                    "https://images.thestar.com/zmNaIUH4viBNAAfaInyOcD1cumw=/1280x1024/smart/filters:cb(1609510078418)/https://www.thestar.com/content/dam/thestar/life/together/pets/2020/12/27/dean-a-toronto-basset-hound-is-a-laziness-guru-what-can-he-teach-us/online27.jpg");                                                                                                
           
            //add medical history for dog 1
            var medicalHistory1 = svc.CreateMedicalHistory(d1.Id,"Painkillers given","Broken leg");
            var medicalHistory2 = svc.CreateMedicalHistory(d1.Id,"Anti-inflammation tablets to take once a week","Arthritis");
            var medicalHistory3 = svc.CreateMedicalHistory(d1.Id,"Blood thinners given, to take once daily","Surgery on heart"); 

            //add medical history for dog 2
            var medicalHistory4 = svc.CreateMedicalHistory(d2.Id,"N/A", "Got neutered and required stitches");

            //add medical history for dog 3
            var medicalHistory5 = svc.CreateMedicalHistory(d3.Id,"Painkillers given","Broken leg");
            
             // close dog 1's first ticket 
            svc.CloseMedicalHistoryNote(medicalHistory1.Id, "Broken leg healed after 6 weeks in a cast.");

             // close dog 2's ticket 
            svc.CloseMedicalHistoryNote(medicalHistory4.Id, "Stitches have healed over and been removed.");

            //add users
            var u1 = svc.Register("Guest", "guest@sms.com", "guest", Role.guest);
            var u2 = svc.Register("Administrator", "admin@sms.com", "admin", Role.admin);
            var u3 = svc.Register("Manager", "manager@sms.com", "manager", Role.manager);

            

        }
    }
}
