using System.ComponentModel.DataAnnotations;

namespace TrainingStudent.Models
{
	public class ResetPasswordViewModel
	{
		
		[Required(ErrorMessage = "NewPassword is required")]
		[MinLength(5, ErrorMessage = "Min Character of NewPassword is 5 Character")]
		public string? NewPassword { get; set; }
		[Required(ErrorMessage = "ConfirmPassword is required")]
		[Compare("NewPassword", ErrorMessage = "ConfirmPassword not the same of Password")]
		public string? ConfirmPassword { get; set; }
		//[Required(ErrorMessage = "Email is required")]
		//[EmailAddress(ErrorMessage = "Invalid email address")]
		public string? Email { get; set; }
		public string? Token { get; set; }
		
	}
}
