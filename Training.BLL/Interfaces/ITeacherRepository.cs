using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.DAL.Entities;

namespace Training.BLL.Interfaces
{
    public interface ITeacherRepository:IGenericRepository<Teacher>
    {
        public Task<IEnumerable<Teacher>> SearchTeacher(string value);
    }
}
