using System;
using System.ComponentModel.DataAnnotations;
using FMS.Data.Validators; //allows access to URL resource

namespace FMS.Data.Models
{
    public class SponsorDog
    {   
        public int Id { get; set; }
       
        // suitable sponsor dog properties/relationships
        [Required]
        public string Breed {get;set;}

        [Required]
        public string Name{get;set;}

        [Required]
        public int Age {get;set;}

        [Required]
        public string ChipNumber {get; set;}

        [Display(Name = "Photo")]
        [UrlResource]
        [Required]
        public string PhotoUrl { get; set; }

        public string ReasonForSponsor {get; set;} 

         //Relationship 1-N Sponsorship note
        public IList<Sponsorship> Sponsorships {get; set;} = new List<Sponsorship>(); 

    }
}