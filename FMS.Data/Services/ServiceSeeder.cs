using System;
using System.Text;
using System.Collections.Generic;
using FMS.Data.Models;

namespace FMS.Data.Services
{
    public static class RehomingServiceSeeder
    {
        // use this class to seed the database with dummy test data using an IRehomingService
        public static void Seed(IRehomingService svc)
        {
            //wipe and recreate the database
            svc.Initialise();

            //========== Add seed data ==========

            //add dogs
            var d1 = svc.AddDog("Cocker Spaniel","Freddie","145C",2,"Freddie is a handsome Cocker Spaniel looking for a home that can provide him with lots of physical and mental enrichment. He is a very intelligent boy who loves playing with his toys and going out exploring new places.",
                                     "https://www.pdsa.org.uk/media/8264/cocker-spaniel-outdoors-gallery-1-min.jpg?anchor=center&mode=crop&quality=100&height=500&bgcolor=fff&rnd=132204646460000000");
            var d2 = svc.AddDog("Sprocker Spaniel","Margo","863F",1,"Margo is a loving and affectionate girl on the lookout for a special home to call her own. She absolutely adores people and is hoping for a home that will shower her with all the cuddles and fuss she desires (in addition to toys and tasty treats, of course!).",
                                     "https://sprockerlovers.com/wp-content/uploads/2020/10/9450861e-19e3-43f3-b784-71a368cd8b57-1024x1024.jpeg");
            var d3 = svc.AddDog("Shih Tzu","Poppie","937B",2,"Poppie is a beautiful Shih Tzu looking for a calm and patient home to settle her paws. She is a very foodie girl who loves a tasty treat and loves using her nose and learning new things. She is an active girl who likes being on the go, exploring and sniffing in quieter areas and enjoys sitting by her human companions and having gentle fusses once she has gotten to know you.",
                                    "https://patterjack.com/wp-content/uploads/2021/11/shih_tzu_article_c.jpg");
            var d4 = svc.AddDog("Beagle","Georgie","275H",3,"Georgie is looking for a quiet home where she can relax and put her paws up in her golden years. Georgie enjoys spending time with known people, although can be quite worried at first, so will need to be given space initially so that she can come round in her own time. Because of this she would like a home with minimal visitors. Georgie likes a couple of short walks a day and is happy to say hello to other dogs when out and about.",
                                    "https://th.bing.com/th/id/R.53abca2d5c19b770a28205de25977876?rik=95Gcy31wNrWN4Q&riu=http%3a%2f%2fwww.petguide.com%2fwp-content%2fuploads%2f2013%2f02%2fbeagle-1.jpg&ehk=O5TbIG8SdlQUhyXhQaWVMkuQG4NXzpzBd6tcvIcj8xw%3d&risl=&pid=ImgRaw&r=0");
           
            var d5 = svc.AddDog("Schnauzer","Max","469B",4,"Max likes having his own space and can sometimes be uncomfortable being handled, he would like a home with calm owners and no children. He is a happy little man and he loves his walks! He likes to explore and have a sniff in amongst the leaves and the tall grass. Max would need to be the only pet in the home as he prefers the company of his family rather than other dogs.",
                                    "https://media-be.chewy.com/wp-content/uploads/2021/06/24103725/MiniatureSchnauzer-FeaturedImage-1024x615.jpg");

            var d6 = svc.AddDog("Husky","Koda","378D",7,"Koda is a sweetheart who loves people and he is an absolute pleasure to care for.He is good to be left for a few hours and good to travel. Koda loves his home comforts, he melts on the sofa and is waiting to melt your heart!",
                                    "https://cdn.britannica.com/84/232784-050-1769B477/Siberian-Husky-dog.jpg");
            var d7 = svc.AddDog("Bichon Frise","Flossy","267H",5,"Flossy is looking for a moderately busy, adult-only home where she can have access to her own secure garden so that she's got a chance to explore and play. Flossy needs a home with no visiting children and would appreciate not having lots of people coming and going whilst she settles in. Her leaving hours need to be built up in a gradual way so that she's got a chance to get used to being in new surroundings. ",
                                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSyv_t1dDFRw7dCgK3aaE0ap74nuJxqcJKijbJZmtwiVW0aye9cVXtr-jAg5nkUvgY_dic&usqp=CAU");
            var d8 = svc.AddDog("Grey Hound","Finn","244G",9,"Finn is an ex racing Greyhound and most likely has never been in a home so he will need help settling into a domestic life. Ideally he would have access to his own secure garden for house training and zoomies whenever he likes. ",
                                    "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5d/Italian_Greyhound_standing_gray_%28cropped%29.jpg/1200px-Italian_Greyhound_standing_gray_%28cropped%29.jpg");
            var d9 = svc.AddDog("Bassett Hound","Winston","463E",7,"Winston is looking for a quiet, adult only home. He's a friendly boy and enjoys a fuss on his terms. He will need his own area of the home where he can go for alone time when he wants it, and also where he can sleep without being disturbed.",
                                    "https://images.thestar.com/zmNaIUH4viBNAAfaInyOcD1cumw=/1280x1024/smart/filters:cb(1609510078418)/https://www.thestar.com/content/dam/thestar/life/together/pets/2020/12/27/dean-a-toronto-basset-hound-is-a-laziness-guru-what-can-he-teach-us/online27.jpg");                                                                                                
           
            //add medical history for dog 1
            var medicalHistory1 = svc.CreateMedicalHistory(d1.Id,"Painkillers given","Broken leg");
            var medicalHistory2 = svc.CreateMedicalHistory(d1.Id,"Anti-inflammation tablets to take once a week","Arthritis");
            var medicalHistory3 = svc.CreateMedicalHistory(d1.Id,"Blood thinners given, to take once daily","Surgery on heart"); 

            //add medical history for dog 2
            var medicalHistory4 = svc.CreateMedicalHistory(d2.Id,"N/A", "Got neutered and required stitches");

            //add medical history for dog 3
            var medicalHistory5 = svc.CreateMedicalHistory(d3.Id,"Painkillers given","Broken leg");

            //add adoption application for dog 1
            var adoptionApplication1 = svc.CreateAdoptionApplication(d1.Id,"Sue Wilson","suewilson@gmail.com","07557228216","Family of 5, both parents retired so can provide a loving home for Freddie");
            
            //Approve dog 1's adoption application
            svc.ApproveAdoptionApplication(adoptionApplication1.Id, "Not suitable to rehome the dog, another dog is required in the house.");

             //Close dog 1's first medical history note 
            svc.CloseMedicalHistoryNote(medicalHistory1.Id, "Broken leg healed after 6 weeks in a cast.");

             //Close dog 2's medical history note 
            svc.CloseMedicalHistoryNote(medicalHistory4.Id, "Stitches have healed over and been removed.");

            //add users
            var u1 = svc.Register("Guest", "guest@sms.com", "guest", Role.guest);
            var u2 = svc.Register("Administrator", "admin@sms.com", "admin", Role.admin);
            var u3 = svc.Register("Manager", "manager@sms.com", "manager", Role.manager);


        }
    }
}
