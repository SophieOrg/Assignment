using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FMS.Web.Models
{
    public class TicketCreateViewModel
    {
        // selectlist of dogs (id, name)       
        public SelectList Dogs { set; get; }

        // Collecting DogId and Issue in Form
        [Required(ErrorMessage = "Please select a Dog")]
        [Display(Name = "Select Dog")]
        public int DogId { get; set;}

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Report { get; set;}
        
        [Required]
        public string Medication {get; set;}
    }

}