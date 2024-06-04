using AutoMapper;
using Training.DAL.Entities;
using TrainingStudent.Models;

namespace TrainingStudent.Mappers
{
    public class CourseStudentProfile:Profile
    {
        public CourseStudentProfile()
        {
            CreateMap<CourseStudentViewModel,CourseStudent>().ReverseMap();
        }
    }
}
