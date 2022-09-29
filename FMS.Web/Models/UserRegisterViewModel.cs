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
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [Display(Name = "Password")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters in length and contain atleast 3 of the following 4 types of characters: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords don't match, please try again !")]
        [Display(Name = "Confirm Password")]  
        public string PasswordConfirm  { get; set; }

        [Required]
        public Role Role { get; set; } = Role.guest;

        [Required]
        public string Name { get; set; }

    }
}