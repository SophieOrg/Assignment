using System;
using Xunit;
using FMS.Data.Models;
using FMS.Data.Services;

namespace FMS.Test
{

    public class ServiceTests
    {
        private readonly IRehomingService svc;


        public ServiceTests()
        {
            // general arrangement
            svc = new RehomingServiceDb();
          
            // ensure data source is empty before each test
            svc.Initialise();
        }

        // ========================== Dog Management System Tests =========================
        [Fact] 
        public void Dog_AddDog_WhenDuplicateChip_ShouldReturnNull()
        {
            // act  
            var v1 = svc.AddDog("Sprocker Spaniel","Margo","PG56 BTQ",1,"Margo is a loving and affectionate girl on the lookout for a special home to call her own. She absolutely adores people and is hoping for a home that will shower her with all the cuddles and fuss she desires (in addition to toys and tasty treats, of course!).",
                                     "https://i.pinimg.com/originals/f1/c3/53/f1c353c73e44e1b5087dd56298438089.jpg");
            // this is a duplicate as the chip number is same as previous dog
            var v2 = svc.AddDog("Sprocker Spaniel","Margo","PG56 BTQ",1,"Margo is a loving and affectionate girl on the lookout for a special home to call her own. She absolutely adores people and is hoping for a home that will shower her with all the cuddles and fuss she desires (in addition to toys and tasty treats, of course!).",
                                     "https://i.pinimg.com/originals/f1/c3/53/f1c353c73e44e1b5087dd56298438089.jpg");
            
            // assert
            Assert.NotNull(v1); // this dog should have been added correctly(if test returns notnull,
                                // dog added because dog not found with that chip number).
            Assert.Null(v2); // This dog should NOT have been added        
        }

        [Fact]
        public void Dog_AddDog_WhenNone_ShouldSetAllProperties()
        {
            // act 
            var added = svc.AddDog("Pug", "Lulu", "537", 3, "Very friendly little girl, enjoys walks but also loves to snooze on the sofa watching TV", "https://cdn.britannica.com/35/233235-050-8DED07E3/Pug-dog.jpg");
            
            // retrieve dog just added by using the Id returned by EF
            var d = svc.GetDog(added.Id);

            // assert - that dog is not null
            Assert.NotNull(d);
            
            // now assert that the properties were set properly
            Assert.Equal(d.Id, d.Id);
            Assert.Equal("Pug", d.Breed);
            Assert.Equal("Lulu", d.Name);
            Assert.Equal("537", d.ChipNumber);
            Assert.Equal(3, d.Age);
            Assert.Equal("Very friendly little girl, enjoys walks but also loves to snooze on the sofa watching TV", d.Information);
            Assert.Equal("https://cdn.britannica.com/35/233235-050-8DED07E3/Pug-dog.jpg", d.PhotoUrl);
        }


        [Fact]
        public void Dog_UpdateDog_ThatExists_ShouldSetAllProperties()
        {
            // arrange - create test dog
            var v = svc.AddDog("Sprocker Spaniel","Margo","PG56 BTQ",1,"Margo is a loving and affectionate girl on the lookout for a special home to call her own. She absolutely adores people and is hoping for a home that will shower her with all the cuddles and fuss she desires (in addition to toys and tasty treats, of course!).",
                                     "https://i.pinimg.com/originals/f1/c3/53/f1c353c73e44e1b5087dd56298438089.jpg");
                        
            // act - create a copy and update any dog properties (except Id) 
            var u = new Dog
            {
                Id = v.Id,
                Name = "Margo",
                Breed = "Sprocker Spaniel",
                ChipNumber = "PG56 BRS",
                Age = 2,
                PhotoUrl = ""
            };
            // save updated dog
            svc.UpdateDog(u); 

            // reload updated dog from database into vu
            var vu = svc.GetDog(v.Id);

            // assert
            Assert.NotNull(u);           

            // now assert that the properties were set properly           
            Assert.Equal(u.Name, vu.Name);
            Assert.Equal(u.Breed, vu.Breed);
            Assert.Equal(u.ChipNumber, vu.ChipNumber);
            Assert.Equal(u.Age, vu.Age);
            Assert.Equal(u.PhotoUrl, vu.PhotoUrl);
            
        }

        [Fact] 
        public void Dog_GetAllDogs_WhenNone_ShouldReturn0()
        {   
            //There is no arrange step here as we are not adding any dogs to the database
            // act 
            var dogs = svc.GetDogs();
            var count = dogs.Count;

            // assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void Dog_GetDogs_When3Exist_ShouldReturn3()
        {
            // arrange - add 3 different dogs to database
            var v1 = svc.AddDog("Shih Tzu","Poppie","MQ12",4,"Poppie is a beautiful Shih Tzu looking for a calm and patient home to settle her paws. She is a very foodie girl who loves a tasty treat and loves using her nose and learning new things. She is an active girl who likes being on the go, exploring and sniffing in quieter areas and enjoys sitting by her human companions and having gentle fusses once she has gotten to know you.",
                                    "https://patterjack.com/wp-content/uploads/2021/11/shih_tzu_article_c.jpg");
            var v2 = svc.AddDog("Cocker Spaniel","Freddie","LG67",2,"Freddie is a handsome Cocker Spaniel looking for a home that can provide him with lots of physical and mental enrichment. He is a very intelligent boy who loves playing with his toys and going out exploring new places.",
                                     "https://www.pdsa.org.uk/media/8264/cocker-spaniel-outdoors-gallery-1-min.jpg?anchor=center&mode=crop&quality=100&height=500&bgcolor=fff&rnd=132204646460000000");
            var v3 = svc.AddDog("Sprocker Spaniel","Margo","PG56",1,"Margo is a loving and affectionate girl on the lookout for a special home to call her own. She absolutely adores people and is hoping for a home that will shower her with all the cuddles and fuss she desires (in addition to toys and tasty treats, of course!).",
                                     "https://i.pinimg.com/originals/f1/c3/53/f1c353c73e44e1b5087dd56298438089.jpg");
            // act
            var dogs = svc.GetDogs();
            var count = dogs.Count;

            // assert
            Assert.Equal(3, count);
        }

        [Fact] 
        public void Dog_GetDog_WhenNonExistent_ShouldReturnNull()
        {
            // act 
            var dog = svc.GetDog(1); // non existent Dog

            // assert
            Assert.Null(dog);
        }

        [Fact] 
        public void Dog_GetDog_ThatExists_ShouldReturnDog()
        {
            // act 
            var d = svc.AddDog("Pug", "Lulu", "537", 3, "Very friendly little girl, enjoys walks but also loves to snooze on the sofa watching TV", "https://cdn.britannica.com/35/233235-050-8DED07E3/Pug-dog.jpg");

            var nd = svc.GetDog(d.Id);

            // assert
            Assert.NotNull(nd);
            Assert.Equal(d.Id, nd.Id);
        }


         [Fact]
        public void Dog_DeleteDog_ThatExists_ShouldReturnTrue()
        {
            // act - add the dog to the database then delete the dog from the database
            var v1 = svc.AddDog("Shih Tzu","Poppie","MQ12 YIS",3,"Poppie is a beautiful Shih Tzu looking for a calm and patient home to settle her paws. She is a very foodie girl who loves a tasty treat and loves using her nose and learning new things. She is an active girl who likes being on the go, exploring and sniffing in quieter areas and enjoys sitting by her human companions and having gentle fusses once she has gotten to know you.",
                                    "https://patterjack.com/wp-content/uploads/2021/11/shih_tzu_article_c.jpg");
            var deleted = svc.DeleteDog(v1.Id);

            // try to retrieve deleted dog
            var v = svc.GetDog(v1.Id);

            // assert
            Assert.True(deleted); // delete dog should return true
            Assert.Null(v);      // v should be null as the dog cannot be retrieved
        }

         [Fact]
        public void Dog_DeleteDog_ThatDoesntExist_ShouldReturnFalse()
        {
            // act 	
            var deleted = svc.DeleteDog(0);

            // assert
            Assert.False(deleted);
        }  

         [Fact]
        public void Dog_UpdateDog_ThatExistsWithNewChipNumber_ShouldWork()
        {
            // arrange
            var v1 = svc.AddDog("Shih Tzu","Poppie","MQ12",3,"Poppie is a beautiful Shih Tzu looking for a calm and patient home to settle her paws. She is a very foodie girl who loves a tasty treat and loves using her nose and learning new things. She is an active girl who likes being on the go, exploring and sniffing in quieter areas and enjoys sitting by her human companions and having gentle fusses once she has gotten to know you.",
                                    "https://patterjack.com/wp-content/uploads/2021/11/shih_tzu_article_c.jpg");

            // act
            // create a copy of added dog and change chip number
            var v = new Dog
            {   
                Id = v1.Id,
                Breed = v1.Breed,
                Name = v1.Name,
                ChipNumber = v1.ChipNumber,
                Age = v1.Age,
                PhotoUrl = v1.PhotoUrl              
            };
            // update this dog
            svc.UpdateDog(v);

            // now load the dog and verify chip number was updated
            var su = svc.GetDog(v.Id);

            // assert
            Assert.Equal(v.ChipNumber, su.ChipNumber);
        }

        [Fact]
        public void Dog_UpdateDog_ThatExistsWithAgePlusOne_ShouldWork()
        {
            // arrange
            var added = svc.AddDog("Pug", "Lulu", "537", 3, "Very friendly little girl, enjoys walks but also loves to snooze on the sofa watching TV", "https://cdn.britannica.com/35/233235-050-8DED07E3/Pug-dog.jpg");

            // act
            // create a copy of added dog and increment age by 1
            var d = new Dog {
                Id = added.Id,
                Breed = added.Breed,
                Name = added.Name,
                ChipNumber = added.ChipNumber,
                Age =  added.Age + 1,
                Information = added.Information,     
                PhotoUrl = added.PhotoUrl          
            };
            // update this student
            svc.UpdateDog(d);

            // now load the student and verify age was updated
            var du = svc.GetDog(d.Id);

            // assert
            Assert.Equal(d.Age, du.Age);
        }

        [Fact] 
        public void Dog_GetDog_ThatExistsWithMedNotes_ShouldReturnDogWithMedNotes()
        {
            // arrange 
            var d = svc.AddDog("Pug", "Lulu", "537", 2, "", "");
            svc.CreateMedicalHistory(d.Id,DateTime.Parse("12-09-21"), "","Ticket 1");
            svc.CreateMedicalHistory(d.Id,DateTime.Parse("12-09-21"), "", "Ticket 2");
            
            // act
            var dog = svc.GetDog(d.Id);

            // assert
            Assert.NotNull(d);    
            Assert.Equal(2, dog.MedicalHistorys.Count);
        }

        // =============== Medical History Note Tests ================
        
        [Fact] 
        public void MedNote_CreateMedicalHistoryNote_ForExistingDog_ShouldBeCreated()
        {
            // arrange
            var v1 = svc.AddDog("Shih Tzu","Poppie","MQ12",5,"Poppie is a beautiful Shih Tzu looking for a calm and patient home to settle her paws. She is a very foodie girl who loves a tasty treat and loves using her nose and learning new things. She is an active girl who likes being on the go, exploring and sniffing in quieter areas and enjoys sitting by her human companions and having gentle fusses once she has gotten to know you.",
                                    "https://patterjack.com/wp-content/uploads/2021/11/shih_tzu_article_c.jpg");
         
            // act
            var t = svc.CreateMedicalHistory(v1.Id,DateTime.Parse("12-09-21"),"Ellen Barlow","Broken leg");
           
            // assert
            Assert.NotNull(t); //t should return "not null" because an Medical History Note has been successfully created
            Assert.Equal(v1.Id, t.DogId); 
        }
       
       
        [Fact] 
        public void Notes_DeleteMedicalHistoryNote_WhenExists_ShouldReturnTrue()
        {
            // arrange
            var v1 = svc.AddDog("Shih Tzu","Poppie","MQ12 YIS",6,"Poppie is a beautiful Shih Tzu looking for a calm and patient home to settle her paws. She is a very foodie girl who loves a tasty treat and loves using her nose and learning new things. She is an active girl who likes being on the go, exploring and sniffing in quieter areas and enjoys sitting by her human companions and having gentle fusses once she has gotten to know you.",
                                    "https://patterjack.com/wp-content/uploads/2021/11/shih_tzu_article_c.jpg");
            var t = svc.CreateMedicalHistory(v1.Id,DateTime.Parse("12-09-21"),"Ellen Barlow","Broken leg");

            // act
            var deleted = svc.DeleteMedicalHistoryNote(t.Id);     // delete medical history note   
            
            // assert
            Assert.True(deleted);                    // medical history note should be deleted
        } 


        [Fact] // --- GetMedNote should include Dog
        public void MedNote_GetMedNote_WhenExists_ShouldReturnMedNoteAndDog()
        {
            // arrange
            var d = svc.AddDog("Pug", "Lulu", "537", 3, "", "");
            var t = svc.CreateMedicalHistory(d.Id,DateTime.Parse("12-09-21"), "medication goes here","Dummy Medical history report");

            // act
            var medNote = svc.GetMedicalHistory(t.Id);

            // assert
            Assert.NotNull(medNote);
            Assert.NotNull(medNote.Dog);
            Assert.Equal(d.Name, medNote.Dog.Name); 
        }

        [Fact] // --- GetOngoingMedicalHistoryNotes when two added should return two 
        public void MedNote_GetOngoingMedNotes_WhenTwoAdded_ShouldReturnTwo()
        {
            // arrange
            var d = svc.AddDog("Pug", "Lulu", "537", 3, "", "");
            var m1 = svc.CreateMedicalHistory(d.Id,DateTime.Parse("12-09-21"),"", "Dummy Medical history note 1");
            var m2 = svc.CreateMedicalHistory(d.Id,DateTime.Parse("12-09-21"),"", "Dummy Medical history note 2");

            // act
            var ongoing = svc.GetOngoingMedicalHistoryNotes();

            // assert
            Assert.Equal(2,ongoing.Count);                        
        }

        [Fact] 
        public void MedNote_CloseMedNote_WhenOngoing_ShouldReturnMedNote()
        {
            // arrange
            var d = svc.AddDog("Pug", "Lulu", "537", 3, "", "");
            var m = svc.CreateMedicalHistory(d.Id,DateTime.Parse("12-09-21"), "","Dummy medical history note");

            // act
            var r = svc.CloseMedicalHistoryNote(m.Id, "Resolved");

            // assert
            Assert.NotNull(r);              // verify closed medical history note is returned          
            Assert.False(r.Active);
            Assert.Equal("Resolved",r.Resolution);
        }

        [Fact] 
        public void MedNote_CloseMedNote_WhenAlreadyClosed_ShouldReturnNull()
        {
            // arrange
            var d = svc.AddDog("Pug", "Lulu", "537", 3, "", "");
            var m = svc.CreateMedicalHistory(d.Id,DateTime.Parse("12-09-21"),"", "Dummy Ticket");

            // act
            var closed = svc.CloseMedicalHistoryNote(m.Id, "Solved");     // close "Ongoing" medical history note    
            closed = svc.CloseMedicalHistoryNote(m.Id,"Solved");         // close "Cured" medical history note

            // assert
            Assert.Null(closed);                    // no medical history note returned as already marked as "Cured"(closed)
        }

        // ================= Adoption Application Tests ==================
        [Fact] 
        public void AdoptionApplication_CreateAdoptionApplication_ForExistingDog_ShouldBeCreated()
        {
            // arrange
            var v1 = svc.AddDog("Shih Tzu","Poppie","MQ12",5,"Poppie is a beautiful Shih Tzu looking for a calm and patient home to settle her paws. She is a very foodie girl who loves a tasty treat and loves using her nose and learning new things. She is an active girl who likes being on the go, exploring and sniffing in quieter areas and enjoys sitting by her human companions and having gentle fusses once she has gotten to know you.",
                                    "https://patterjack.com/wp-content/uploads/2021/11/shih_tzu_article_c.jpg");
         
            // act
            var a = svc.CreateAdoptionApplication(v1.Id,"Ellen Barlow","ellen31@gmail.com","07557228216","Living in an apartment with one cat.  There is a fenced off shared garden for the apartment block.");
           
            // assert
            Assert.NotNull(a); //a should return "not null" because an Adoption Application has been successfully created
            Assert.Equal(v1.Id, a.DogId); 
        }
       
       
        [Fact] 
        public void AdoptionApplication_DeleteAdoptionApplication_WhenExists_ShouldReturnTrue()
        {
            // arrange
            var v1 = svc.AddDog("Shih Tzu","Poppie","MQ12 YIS",6,"Poppie is a beautiful Shih Tzu looking for a calm and patient home to settle her paws. She is a very foodie girl who loves a tasty treat and loves using her nose and learning new things. She is an active girl who likes being on the go, exploring and sniffing in quieter areas and enjoys sitting by her human companions and having gentle fusses once she has gotten to know you.",
                                    "https://patterjack.com/wp-content/uploads/2021/11/shih_tzu_article_c.jpg");
            var a = svc.CreateAdoptionApplication(v1.Id,"Ellen Barlow","ellen31@gmail.com","07557228216","Living in an apartment with one cat.  There is a fenced off shared garden for the apartment block.");

            // act
            var deleted = svc.DeleteAdoptionApplication(a.Id);     // delete adoption application  
            
            // assert
            Assert.True(deleted);                    // adoption application should be deleted
        } 


        [Fact] // --- GetAdoptionApplication should include Dog
        public void AdoptionApplication_GetAdoptionApplication_WhenExists_ShouldReturnAdoptionApplicationAndDog()
        {
            // arrange
            var d = svc.AddDog("Pug", "Lulu", "537", 3, "", "");
            var a = svc.CreateAdoptionApplication(d.Id,"Ellen Barlow","ellen31@gmail.com","07557228216","Living in an apartment with one cat.  There is a fenced off shared garden for the apartment block.");

            // act
            var adoptionApplication = svc.GetAdoptionApplication(a.Id);

            // assert
            Assert.NotNull(adoptionApplication);
            Assert.NotNull(adoptionApplication.Dog);
            Assert.Equal(d.Name, adoptionApplication.Dog.Name); 
        }

        [Fact] // --- GetAwaitingAdoptionApplications when two added should return two 
        public void AdoptionApplication_GetAwaitingAdoptionApplications_WhenTwoAdded_ShouldReturnTwo()
        {
            // arrange
            var d = svc.AddDog("Pug", "Lulu", "537", 3, "", "");
            var a1 = svc.CreateAdoptionApplication(d.Id,"Ellen Barlow","ellen31@gmail.com","07557228216","Living in an apartment with one cat.  There is a fenced off shared garden for the apartment block.");
            var a2 = svc.CreateAdoptionApplication(d.Id,"Euan Black","euan17@gmail.com","07557341874","Family of 5 living in a house in the countryside.  Large fenced off grassy area that would be perfect for Lulu to run about in.");

            // act
            var ongoing = svc.GetValidAdoptionApplications();

            // assert
            Assert.Equal(2,ongoing.Count);                        
        }

        [Fact] 
        public void AdoptionApplication_ApproveAdoptionApplication_WhenAwaiting_ShouldReturnAdoptionApplication()
        {
            // arrange
            var d = svc.AddDog("Pug", "Lulu", "537", 3, "", "");
            var a = svc.CreateAdoptionApplication(d.Id,"Ellen Barlow","ellen31@gmail.com","07557228216","Living in an apartment with one cat.  There is a fenced off shared garden for the apartment block.");

            // act
            var r = svc.ApproveAdoptionApplication(a.Id, "Resolved");

            // assert
            Assert.NotNull(r);              //verify approved adoption application is returned          
            Assert.False(r.Active);
            Assert.Equal("Resolved",r.Resolution);
        }

        [Fact] 
        public void AdoptionApplication_ApproveAdoptionApplication_WhenAlreadyApproved_ShouldReturnNull()
        {
            // arrange
            var d = svc.AddDog("Pug", "Lulu", "537", 3, "", "");
            var m = svc.CreateAdoptionApplication(d.Id,"Ellen Barlow","ellen31@gmail.com","07557228216","Living in an apartment with one cat.  There is a fenced off shared garden for the apartment block.");

            // act
            var approved = svc.ApproveAdoptionApplication(m.Id, "approved");     // approve "awaiting" adoption application   
            approved = svc.ApproveAdoptionApplication(m.Id,"approved");         // close "approved" adoption application

            // assert
            Assert.Null(approved);                    // no adoption application returned as already marked as "Approved"
        }

        
        // =================  User Tests ===========================
        
        [Fact] // --- Register Valid User test
        public void User_Register_WhenValid_ShouldReturnUser()
        {
            // arrange 
            var reg = svc.Register("XXX", "xxx@email.com", "admin", Role.volunteer);
            
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
            var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.volunteer);
            
            // act
            var s2 = svc.Register("XXX", "xxx@email.com", "admin", Role.volunteer);

            // assert
            Assert.NotNull(s1);
            Assert.Null(s2);
        } 

        [Fact] // --- Authenticate Invalid Test
        public void User_Authenticate_WhenInValidCredentials_ShouldReturnNull()
        {
            // arrange 
            var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.volunteer);
        
            // act
            var user = svc.Authenticate("xxx@email.com", "guest");
            // assert
            Assert.Null(user);

        } 

        [Fact] // --- Authenticate Valid Test
        public void User_Authenticate_WhenValidCredentials_ShouldReturnUser()
        {
            // arrange 
            var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.volunteer);
        
            // act
            var user = svc.Authenticate("xxx@email.com", "admin");
            
            // assert
            Assert.NotNull(user);
        } 



    }
}