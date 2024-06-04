using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.DAL.Entities;

namespace Training.DAL.Context
{
    public class SchoolingContext:IdentityDbContext<ApplicationUser,IdentityRole,string>
    {
        public SchoolingContext(DbContextOptions<SchoolingContext> options) : base(options) { }
        public DbSet<Department>Departments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentTeacher> StudentTeachers { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }
        

    }

  
}
