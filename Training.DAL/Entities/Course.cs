using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.DAL.Entities
{
    public class Course
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Duration { get; set; }
        public bool IsDeleted { get; set; } = false;


        [Required]
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
        public virtual ICollection<CourseStudent> CoursesStudents { get; set; } = new HashSet<CourseStudent>();//add navigation properties to simplify querying and navigating the relationships.



    }
}
