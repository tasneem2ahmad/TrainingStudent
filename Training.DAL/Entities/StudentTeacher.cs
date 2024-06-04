using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.DAL.Entities
{
    public class StudentTeacher
    {
        public int Id { get; set; }
        public int StudentID { get; set; }
        public Student Student { get; set; }

        
        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }

    }
}
