using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.BLL.Interfaces;

namespace Training.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        public IDepartmentRepository DepartmentRepository { get  ; set; }
        public ITeacherRepository TeacherRepository { get; set ; }
        public IStudentRepository StudentRepository { get; set; }
        public ICourseRepository CourseRepository { get; set; }
        public ICourseStudentRepository CourseStudentRepository { get; set; }

        public UnitOfWork(ITeacherRepository teacherRepository, IDepartmentRepository departmentRepository,IStudentRepository studentRepository, ICourseRepository courseRepository,ICourseStudentRepository courseStudentRepository)
        {
            DepartmentRepository = departmentRepository;
            TeacherRepository = teacherRepository;
            StudentRepository = studentRepository;
            CourseRepository = courseRepository;
            CourseStudentRepository = courseStudentRepository;
        }
    }
}
