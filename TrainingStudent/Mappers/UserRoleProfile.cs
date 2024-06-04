using AutoMapper;
using Training.DAL.Entities;
using TrainingStudent.Models;

namespace TrainingStudent.Mappers
{
    public class UserRoleProfile:Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserViewModel, ApplicationUser>().ReverseMap();
        }
    }
}
