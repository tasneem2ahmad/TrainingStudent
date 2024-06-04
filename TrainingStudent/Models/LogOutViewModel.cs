﻿using System.ComponentModel.DataAnnotations;

namespace TrainingStudent.Models
{
    public class LogOutViewModel
    {
		public string? UserName { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[MinLength(5, ErrorMessage = "Min Character of Password is 5 Character")]
		public string? Password { get; set; }

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string? Email { get; set; }

		public string? Token { get; set; }
        [Required]
        public bool IsTokenRevoked { get; set; }
    }
}
