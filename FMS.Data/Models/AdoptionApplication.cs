using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FMS.Data.Models
{   
    //used in adoption application search feature
    public enum AdoptionApplicationRange {AWAITING,APPROVED,ALL}
    public class AdoptionApplication
    {
        public int Id { get; set; }
        
        // suitable adoption application properties/relationships

        [Required]
        public string Name{get;set;}

        [Required]
        public string Email{get;set;}

        [Required]
        public string PhoneNumber{get;set;}

        [Required]
        [StringLength(300,MinimumLength = 5)]
        public string Information {get; set;}

        public bool Active {get; set;} = true; 

        //Adoption application owned by a dog
        public int DogId {get; set;}  //foreign key

        [JsonIgnore]
        public Dog Dog {get; set;}  //navigation property
    

    }

}
