using System;
using System.ComponentModel.DataAnnotations;
using SMS.Data.Validators;

namespace FMS.Data.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        
        // suitable vehicle properties/relationships
        [Required]
        public string Make {get;set;}

        [Required]
        public string Model{get;set;}

        [Required]
        public int Year{get;set;}

        [Required]
        public string Registration{get;set;}

        [Required]
        public string FuelType{get;set;}

        [Required]
        public string BodyType{get;set;}

        [Required]
        public string TransmissionType{get;set;}

        [Required]
        public int CC {get;set;}

        [Required]
        public int No0fDoors {get;set;}

        [Required]
        public DateOnly MotDue{get;set;}

        [Display(Name = "Photo")]
        [Required]
        [UrlResource]
        public string PhotoUrl { get; set; }
        
        [Display(Name = "Price(£)")]
        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public double Price{get; set;} 

        //Relationship 1-N MOT ticket
        public IList<Mot> Mots {get; set;} = new List<Mot>();

    }

}
