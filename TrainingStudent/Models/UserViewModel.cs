using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Training.DAL.Entities;

namespace TrainingStudent.Models
{
	public class UserViewModel
	{
        public string Id { get; set; }
        public string? UserName { get; set; }
        
        public string? Password { get; set; }
        
        public string? ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Agree to our Terms ")]
        public bool IsAgree { get; set; }

        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Assign Role ....!")]
        public string Role { get; set; }



    }
}
