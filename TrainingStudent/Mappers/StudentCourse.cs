using AutoMapper;
using Training.DAL.Entities;
using TrainingStudent.Models;

namespace TrainingStudent.Mappers
{
    public class StudentCourse:Profile
    {
        public StudentCourse()
        {
            CreateMap<StudentWithCourseViewModel,CourseStudent>().ReverseMap();
        }
    }
}
