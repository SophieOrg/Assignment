using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FMS.Web.Models
{
    public class TicketCreateViewModel
    {
        // selectlist of students (id, name)       
        public SelectList Dogs { set; get; }

        // Collecting StudentId and Issue in Form
        [Required(ErrorMessage = "Please select a student")]
        [Display(Name = "Select Student")]
        public int DogId { get; set; }

        public DateTime On { get; set; }

        public string Vet { get; set; }

        public string Status { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Report { get; set; }
    }

}