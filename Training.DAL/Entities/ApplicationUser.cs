using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.DAL.Entities
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		public bool IsAgree { get; set; }
        //[Required]
        //public virtual ICollection<IdentityUserRole<string>>? UserRoles { get; set; }
        [Required]

        public string Role { get; set; }
        [Required]
        public bool IsTokenRevoked { get; set; }
    }
}
