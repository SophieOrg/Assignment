using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FMS.Web.Models
{
    public class TicketCreateViewModel
    {
        // selectlist of students (id, name)       
        public SelectList Dogs { set; get; }

        // Collecting StudentId and Issue in Form
        [Required(ErrorMessage = "Please select a Dog")]
        [Display(Name = "Select Dog")]
        public int DogId { get; set;}

        public string Vet {get;set;}

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Report { get; set;}
    }

}