using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.DAL.Entities
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string PhoneNumber { get; set; }
        
        [Required]
        public string City { get; set; }
        [Required]
        public int YearofSchool { get; set; }
        [Required]
        public string SchoolGrade { get; set; }
        [Required]
        public int DepartmentID {  get; set; }
        public Department Department { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
        public virtual ICollection<Teacher> Teachers { get; set; } =new HashSet<Teacher>();
        public virtual ICollection<CourseStudent> CoursesStudents { get; set; } = new HashSet<CourseStudent>();//add navigation properties to simplify querying and navigating the relationships.

    }
}
