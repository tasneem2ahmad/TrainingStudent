using System.ComponentModel.DataAnnotations;
using Training.DAL.Entities;

namespace TrainingStudent.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Teacher Name is Required")]
        [MinLength(3, ErrorMessage = "MinLength contains 3 Characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PhoneNumber is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be exactly 11 numbers")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "YearofSchool is required")]
        public int YearofSchool { get; set; }
        [Required(ErrorMessage = "SchoolGrade is required")]
        public string SchoolGrade { get; set; }
        [Required(ErrorMessage = "Department is required")]
        public int DepartmentID { get; set; }
        public Department? Department { get; set; }
        public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
        public virtual ICollection<Teacher> Teachers { get; set; } = new HashSet<Teacher>();
        public virtual ICollection<CourseStudent> CoursesStudents { get; set; } = new HashSet<CourseStudent>();//add navigation properties to simplify querying and navigating the relationships.


    }
}
