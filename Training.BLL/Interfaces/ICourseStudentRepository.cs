using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.DAL.Entities;

namespace Training.BLL.Interfaces
{
    public interface ICourseStudentRepository:IGenericRepository<CourseStudent>
    {
        public Task<IEnumerable<CourseStudent>> SearchCourseStudent(string value);
    }
}
