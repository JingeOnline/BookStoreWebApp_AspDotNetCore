using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Models
{
    public class SignUpUserModel
    {
        [Required(ErrorMessage ="Please enter your email.")]
        [Display(Name ="Email")]
        [EmailAddress(ErrorMessage ="The email address is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please enter your password.")]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password does not match.")]
        [Display(Name ="Confirm Password")]
        [Required(ErrorMessage ="Please repeat your password.")]
        public string ConfirmPassword { get; set; }
    }
}
