using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.DAL.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [ForeignKey("Department")]
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        [Range(typeof(DateTime), "1/1/1900", "1/1/1997")]
        public DateTime DateofBirth { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime HireDate { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Range(3000,60000)]
        public decimal Salary { get; set; }
        
        public string ?ImageName { get; set; }

        
        [Required]
        public int CourseTeacherId { get; set; }
        public Course? CourseTeacher { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
