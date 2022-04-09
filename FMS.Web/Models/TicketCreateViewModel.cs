using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FMS.Web.Models
{
    public class TicketCreateViewModel
    {
        // selectlist of students (id, name)       
        public SelectList Vehicles { set; get; }

        // Collecting VehicleId and Report in Form
        [Required(ErrorMessage = "Please select a vehicle")]
        [Display(Name = "Select Vehicle")]
        public int VehicleId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Report { get; set; }
    }

}