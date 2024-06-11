using System.ComponentModel.DataAnnotations;
using Training.DAL.Entities;

namespace TrainingStudent.Models
{
    public class CourseStudentViewModel
    {
        public int Id { get; set; } 
        public int StudentID { get; set; }
        public Student? Student { get; set; }

        public int CourseID { get; set; }
        public Course? Course { get; set; }
        [Required(ErrorMessage = "Enter Student FinalDegree")]
        public decimal FinalDegree { get; set; }
        [Required(ErrorMessage = "Enter Student MidDegree")]
        public decimal MidDegree { get; set; }
        [Required(ErrorMessage = "Enter Student PracticalDegree")]
        public decimal PracticalDegree { get; set; }
        public decimal Max { get; set; } = 100;
        public bool IsDeleted { get; set; }

    }
}
