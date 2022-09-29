using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FMS.Web.Models
{
    public class MedNoteCreateViewModel
    {
        // selectlist of dogs (id, name)       
        public SelectList Dogs { set; get; }

        // Collecting DogId, date created on, report and medication in Form
        [Required(ErrorMessage = "Please select a Dog")]
        [Display(Name = "Select Dog")]
        public int DogId { get; set;}

        public DateTime CreatedOn {get;set;}

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Report { get; set;}
        
        [Required]
        public string Medication {get; set;}
    }

}