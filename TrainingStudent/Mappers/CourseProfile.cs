using AutoMapper;
using Training.DAL.Entities;
using TrainingStudent.Models;

namespace TrainingStudent.Mappers
{
    public class CourseProfile:Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseViewModel,Course>().ReverseMap();
        }
    }
}
