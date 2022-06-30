using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FMS.Data.Models
{   
    public class MedicalHistory
    {
        public int Id { get; set; }
        
        // suitable medical history attributes / relationships are detailed here
        [Required]
        [StringLength(300, MinimumLength = 5)]
        public string Report {get;set;}
        
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime On {get; set;}

        public string Vet {get;set;}
        
        public string Status {get;set;}

        //MOT owned by a user
        public int DogId {get;set;} //foreign key
        
        public Dog Dog {get; set;}  //navigation property


    }
}
