using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TrainingStudent.Models
{
    public class RoleViewModel
    {
        public virtual string? Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name for this role.
        /// </summary>
        [Required(ErrorMessage = "Teacher Name is Required")]
        [MinLength(3, ErrorMessage = "MinLength contains 3 Characters")]
        public virtual string? Name { get; set; }

        /// <summary>
        /// Gets or sets the normalized name for this role.
        /// </summary>
        
        public virtual string? NormalizedName { get; set; }
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }
}
