using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Training.DAL.Entities;

namespace TrainingStudent.Models
{
    public class CourseViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "please enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "please enter Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "please enter Duration")]
        public string Duration { get; set; }

        [ForeignKey("Department")]
        [Required(ErrorMessage ="Department is required")]
        public int DepartmentId { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
        public virtual ICollection<CourseStudent> CoursesStudents { get; set; } = new HashSet<CourseStudent>();//add navigation properties to simplify querying and navigating the relationships.

    }
}
