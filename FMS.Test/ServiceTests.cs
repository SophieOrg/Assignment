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
            var v1 = svc.AddDog("Sprocker Spaniel","Margo","PG56 BTQ",DateTime.Parse("2021-11-25"),"Margo is a loving and affectionate girl on the lookout for a special home to call her own. She absolutely adores people and is hoping for a home that will shower her with all the cuddles and fuss she desires (in addition to toys and tasty treats, of course!).",
                                     "https://i.pinimg.com/originals/f1/c3/53/f1c353c73e44e1b5087dd56298438089.jpg");
            // this is a duplicate as the chip number is same as previous dog
            var v2 = svc.AddDog("Sprocker Spaniel","Margo","PG56 BTQ",DateTime.Parse("2021-11-25"),"Margo is a loving and affectionate girl on the lookout for a special home to call her own. She absolutely adores people and is hoping for a home that will shower her with all the cuddles and fuss she desires (in addition to toys and tasty treats, of course!).",
                                     "https://i.pinimg.com/originals/f1/c3/53/f1c353c73e44e1b5087dd56298438089.jpg");
            
            // assert
            Assert.NotNull(v1); // this dog should have been added correctly(if test returns notnull,
                                // dog added because dog not found with that chip number).
            Assert.Null(v2); // This dog should NOT have been added        
        }

        [Fact]
        public void Dog_UpdateDog_ThatExists_ShouldSetAllProperties()
        {
            // arrange - create test dog
            var v = svc.AddDog("Sprocker Spaniel","Margo","PG56 BTQ",DateTime.Parse("2021-11-25"),"Margo is a loving and affectionate girl on the lookout for a special home to call her own. She absolutely adores people and is hoping for a home that will shower her with all the cuddles and fuss she desires (in addition to toys and tasty treats, of course!).",
                                     "https://i.pinimg.com/originals/f1/c3/53/f1c353c73e44e1b5087dd56298438089.jpg");
                        
            // act - create a copy and update any dog properties (except Id) 
            var u = new Dog
            {
                Id = v.Id,
                Name = "Margo",
                Breed = "Sprocker Spaniel",
                ChipNumber = "PG56 BRS",
                DOB = DateTime.Parse("2010-05-08"),
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
            Assert.Equal(u.DOB, vu.DOB);
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
        public void Dog_GetDogs_When3Exist_ShouldReturn2()
        {
            // arrange - add 3 different dogs to database
            var v1 = svc.AddDog("Shih Tzu","Poppie","MQ12 YIS",DateTime.Parse("2020-06-13"),"Poppie is a beautiful Shih Tzu looking for a calm and patient home to settle her paws. She is a very foodie girl who loves a tasty treat and loves using her nose and learning new things. She is an active girl who likes being on the go, exploring and sniffing in quieter areas and enjoys sitting by her human companions and having gentle fusses once she has gotten to know you.",
                                    "https://patterjack.com/wp-content/uploads/2021/11/shih_tzu_article_c.jpg");
            var v2 = svc.AddDog("Cocker Spaniel","Freddie","LG67 PUT",DateTime.Parse("2022-05-02"),"Freddie is a handsome Cocker Spaniel looking for a home that can provide him with lots of physical and mental enrichment. He is a very intelligent boy who loves playing with his toys and going out exploring new places.",
                                     "https://www.pdsa.org.uk/media/8264/cocker-spaniel-outdoors-gallery-1-min.jpg?anchor=center&mode=crop&quality=100&height=500&bgcolor=fff&rnd=132204646460000000");
            var v3 = svc.AddDog("Sprocker Spaniel","Margo","PG56 BTQ",DateTime.Parse("2021-11-25"),"Margo is a loving and affectionate girl on the lookout for a special home to call her own. She absolutely adores people and is hoping for a home that will shower her with all the cuddles and fuss she desires (in addition to toys and tasty treats, of course!).",
                                     "https://i.pinimg.com/originals/f1/c3/53/f1c353c73e44e1b5087dd56298438089.jpg");
            // act
            var dogs = svc.GetDogs();
            var count = dogs.Count;

            // assert
            Assert.Equal(3, count);
        }

         [Fact]
        public void Dog_DeleteDog_ThatExists_ShouldReturnTrue()
        {
            // act - add the vehicle to the database then delete the vehicle from the database
            var v1 = svc.AddDog("Shih Tzu","Poppie","MQ12 YIS",DateTime.Parse("2020-06-13"),"Poppie is a beautiful Shih Tzu looking for a calm and patient home to settle her paws. She is a very foodie girl who loves a tasty treat and loves using her nose and learning new things. She is an active girl who likes being on the go, exploring and sniffing in quieter areas and enjoys sitting by her human companions and having gentle fusses once she has gotten to know you.",
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
            var v1 = svc.AddDog("Shih Tzu","Poppie","MQ12 YIS",DateTime.Parse("2020-06-13"),"Poppie is a beautiful Shih Tzu looking for a calm and patient home to settle her paws. She is a very foodie girl who loves a tasty treat and loves using her nose and learning new things. She is an active girl who likes being on the go, exploring and sniffing in quieter areas and enjoys sitting by her human companions and having gentle fusses once she has gotten to know you.",
                                    "https://patterjack.com/wp-content/uploads/2021/11/shih_tzu_article_c.jpg");

            // act
            // create a copy of added dog and change chip number
            var v = new Dog
            {   
                Id = v1.Id,
                Breed = v1.Breed,
                Name = v1.Name,
                ChipNumber = v1.ChipNumber,
                DOB = v1.DOB,
                PhotoUrl = v1.PhotoUrl              
            };
            // update this dog
            svc.UpdateDog(v);

            // now load the dog and verify chip number was updated
            var su = svc.GetDog(v.Id);

            // assert
            Assert.Equal(v.ChipNumber, su.ChipNumber);
        }

        // =============== Medical History Note Tests ================
        
        [Fact] 
        public void Notes_CreateMedicalHistoryNote_ForExistingDog_ShouldBeCreated()
        {
            // arrange
            var v1 = svc.AddDog("Shih Tzu","Poppie","MQ12 YIS",DateTime.Parse("2020-06-13"),"Poppie is a beautiful Shih Tzu looking for a calm and patient home to settle her paws. She is a very foodie girl who loves a tasty treat and loves using her nose and learning new things. She is an active girl who likes being on the go, exploring and sniffing in quieter areas and enjoys sitting by her human companions and having gentle fusses once she has gotten to know you.",
                                    "https://patterjack.com/wp-content/uploads/2021/11/shih_tzu_article_c.jpg");
         
            // act
            var t = svc.CreateMedicalHistory(v1.Id,"Ellen Barlow","Broken leg");
           
            // assert
            Assert.NotNull(t); //t should return "not null" because an Medical History Note has been successfully created
            Assert.Equal(v1.Id, t.DogId); 
        }
       
       
        [Fact] 
        public void Notes_DeleteMedicalHistoryNote_WhenExists_ShouldReturnTrue()
        {
            // arrange
            var v1 = svc.AddDog("Shih Tzu","Poppie","MQ12 YIS",DateTime.Parse("2020-06-13"),"Poppie is a beautiful Shih Tzu looking for a calm and patient home to settle her paws. She is a very foodie girl who loves a tasty treat and loves using her nose and learning new things. She is an active girl who likes being on the go, exploring and sniffing in quieter areas and enjoys sitting by her human companions and having gentle fusses once she has gotten to know you.",
                                    "https://patterjack.com/wp-content/uploads/2021/11/shih_tzu_article_c.jpg");
            var t = svc.CreateMedicalHistory(v1.Id,"Ellen Barlow","Broken leg");

            // act
            var deleted = svc.DeleteMedicalHistoryNote(t.Id);     // delete medical history note   
            
            // assert
            Assert.True(deleted);                    // medical history note should be deleted
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