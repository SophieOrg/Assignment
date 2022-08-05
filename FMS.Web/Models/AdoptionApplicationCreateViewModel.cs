using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FMS.Web.Models
{
    public class AdoptionApplicationCreateViewModel
    {
        // selectlist of dogs (id, name)       
        public SelectList Dogs { set; get; }

        // Collecting DogId and Issue in Form
        [Required(ErrorMessage = "Please select a Dog")]
        [Display(Name = "Select Dog")]
        public int DogId { get; set;}

        [Required]
        [StringLength(300, MinimumLength = 5)]
        public string Information { get; set;}

        [Required]
        public string Name {get; set;}

         [Required]
        public string Email {get; set;}

         [Required]
        public string PhoneNumber {get; set;}

    
    }

}