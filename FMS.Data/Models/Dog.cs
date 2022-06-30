using System;
using System.ComponentModel.DataAnnotations;
using FMS.Data.Validators; //allows access to URL resource

namespace FMS.Data.Models
{
    public class Dog
    {
        public int Id { get; set; }
        
        // suitable vehicle properties/relationships
        [Required]
        public string Breed {get;set;}

        [Required]
        public string Name{get;set;}

        [Required]
        public string ChipNumber{get;set;}

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB {get;set;}

        [Display(Name = "Photo")]
        [UrlResource]
        [Required]
        public string PhotoUrl { get; set; }

        //Relationship 1-N Medical History note
        public IList<MedicalHistory> MedicalHistorys {get; set;} = new List<MedicalHistory>();

    }

}
