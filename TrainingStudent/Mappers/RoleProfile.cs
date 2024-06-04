using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Training.DAL.Entities;
using TrainingStudent.Models;

namespace TrainingStudent.Mappers
{
    public class RoleProfile:Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModel, IdentityRole>().ReverseMap();
        }
    }
}
