using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.DAL.Entities
{
    public class Department
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Department Name is Required")]
        [MinLength(3, ErrorMessage = "MinLength contains 3 Characters")]
        public string Name { get; set; }
        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Student> Students { get; set; } = new List<Student>();

    }
}
