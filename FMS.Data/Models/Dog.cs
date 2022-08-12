using System;
using System.ComponentModel.DataAnnotations;
using FMS.Data.Validators; //allows access to UrlResource validator

namespace FMS.Data.Models
{
    public class Dog
    {
        public int Id { get; set; }
        
        // suitable dog properties/relationships
        [Required]
        public string Breed {get;set;}

        [Required]
        public string Name{get;set;}

        [Required]
        public string ChipNumber{get;set;}

        [Required]
        public int Age {get;set;}

        [Display(Name = "Photo")]
        [UrlResource]
        [Required]
        public string PhotoUrl { get; set; }

        public string Information {get; set;}

        //Relationship 1-N Medical History note
        public IList<MedicalHistory> MedicalHistorys {get; set;} = new List<MedicalHistory>();

        //Relationship 1-N Adoption Application
         public IList<AdoptionApplication> AdoptionApplications {get; set;} = new List<AdoptionApplication>();
        
    }

}
