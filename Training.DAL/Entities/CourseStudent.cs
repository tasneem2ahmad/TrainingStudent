using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.DAL.Entities
{
    public class CourseStudent
    {
        public int Id { get; set; }

        public int StudentID { get; set; }
        public Student? Student { get; set; }
        
        public int CourseID { get; set; }
        public Course? Course { get; set; }
        [Required]
        public decimal FinalDegree { get; set; }
        [Required]
        public decimal MidDegree { get; set; }
        [Required]
        public decimal PracticalDegree { get; set; }
        public decimal Max { get; set; } = 100;
    }
}
