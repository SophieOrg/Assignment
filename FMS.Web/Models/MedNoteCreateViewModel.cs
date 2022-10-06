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
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn {get;set;} = DateTime.Now;

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Report { get; set;}
        
        [Required]
        public string Medication {get; set;}
    }

}