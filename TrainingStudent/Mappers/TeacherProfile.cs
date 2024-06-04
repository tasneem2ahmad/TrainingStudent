using AutoMapper;
using Training.DAL.Entities;
using TrainingStudent.Models;

namespace TrainingStudent.Mappers
{
    public class TeacherProfile:Profile
    {
        public TeacherProfile()
        {
            //transfere datatype from TeacherViewModel to Teacher and reverse
            CreateMap<TeacherViewModel, Teacher>().ReverseMap();
        }
    }
}
