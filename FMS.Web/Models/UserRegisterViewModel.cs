using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using FMS.Data.Models;

namespace FMS.Web.Models
{
    // need separate registerview model for register remote validation which is not needed in login
    public class UserRegisterViewModel
    {       
        [Required]
        [EmailAddress]
        [Remote("VerifyEmailAddress", "User")]
        public string Email { get; set; }
 
        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords don't match, try again !")]
        [Display(Name = "Confirm Password")]  
        public string PasswordConfirm  { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        public string Name { get; set; }

    }
}