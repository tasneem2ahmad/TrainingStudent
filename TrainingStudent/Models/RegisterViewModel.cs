using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TrainingStudent.Models
{
    public class RegisterViewModel
    {
		public string ? UserName { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[MinLength(5,ErrorMessage ="Min Character of Password is 5 Character")]
		public string? Password { get; set; }
		[Required(ErrorMessage = "ConfirmPassword is required")]
		[Compare("Password", ErrorMessage = "ConfirmPassword not the same of Password")]
		public string? ConfirmPassword { get; set; }
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string? Email { get; set; }
		[Required(ErrorMessage = "Agree to our Terms ")]
		public bool IsAgree { get; set; }

		public string? PhoneNumber { get; set; }="01005514745";
		public string? Role { get; set; } = "User";

    }
}
