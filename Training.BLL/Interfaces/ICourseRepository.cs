using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.DAL.Entities;

namespace Training.BLL.Interfaces
{
    public interface ICourseRepository:IGenericRepository<Course>
    {
        public Task<IEnumerable<Course>> SearchCourse(string value);
    }
}
