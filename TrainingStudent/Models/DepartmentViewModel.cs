using System.ComponentModel.DataAnnotations;
using Training.DAL.Entities;

namespace TrainingStudent.Models
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Department Name is Required")]
        [MinLength(3, ErrorMessage = "MinLength contains 3 Characters")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
