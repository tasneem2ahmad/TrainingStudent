using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Training.DAL.Entities;

namespace TrainingStudent.Models
{
    public class TeacherViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Teacher Name is Required")]
        [MinLength(3, ErrorMessage = "MinLength contains 3 Characters")]
        public string Name { get; set; }
        [ForeignKey("Department")]
        [Required(ErrorMessage = "DepartmentId is required")]
        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "DateofBirth is required")]
        [Range(typeof(DateTime), "1/1/1900", "1/1/1997", ErrorMessage = "Minimum age requirement is 25 years")]
        public DateTime DateofBirth { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PhoneNumber is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be exactly 11 numbers")]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Salary is required")]
        [DataType(DataType.Currency)]
        [Range(3000, 60000, ErrorMessage = "Salary should be in range of 3000 to 60000")]
        public decimal Salary { get; set; }
        
        public string? ImageName { get; set; }
        [Required(ErrorMessage = "Select file ")]
        public IFormFile Image { get; set; }
        [Required(ErrorMessage ="Course that you will teach is required")]
        public int CourseTeacherId { get; set; }
        public Course? CourseTeacher { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    }
}
