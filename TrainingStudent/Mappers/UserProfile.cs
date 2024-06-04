using AutoMapper;
using Training.DAL.Entities;
using TrainingStudent.Models;

namespace TrainingStudent.Mappers
{
	public class UserProfile:Profile
	{
        public UserProfile()
        {
            CreateMap<RegisterViewModel,ApplicationUser>().ReverseMap();
        }
    }
}
