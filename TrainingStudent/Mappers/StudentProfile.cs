using AutoMapper;
using Training.DAL.Entities;
using TrainingStudent.Models;


namespace TrainingStudent.Mappers
{
    public class StudentProfile:Profile
    {
        public StudentProfile()
        {
            //transfere datatype from StudentViewModel to Student and reverse
            CreateMap<StudentViewModel, Student>().ReverseMap();
        }
    }
}
