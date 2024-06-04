using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.BLL.Interfaces;
using Training.DAL.Context;
using Training.DAL.Entities;

namespace Training.BLL.Repositories
{
    public class TeacherRepository:IGenericRepository<Teacher>,ITeacherRepository
    {
        private readonly SchoolingContext context;
        public TeacherRepository(SchoolingContext context)
        {
            this.context = context;
        }
        public async Task<int> Add(Teacher departmet)
        {
            context.AddAsync(departmet);
            return await context.SaveChangesAsync();
        }
       
        public async Task<int> Delete(Teacher departmet)
        {
            context.Remove(departmet);
            return await context.SaveChangesAsync();
        }

        public async Task< Teacher> Get(int? id)
        {
            return await context.Teachers.FindAsync(id);
        }

        public async Task< IEnumerable<Teacher>> GetAll() =>await context.Teachers.ToListAsync();
        
        public async Task< IEnumerable<Teacher>> SearchTeacher(string value)
        => await context.Teachers.Where(c => c.Name.Contains(value)).ToListAsync();

        public async Task<int> Update(Teacher departmet)
        {
            context.Update(departmet);
            return await context.SaveChangesAsync();
        }
    }
}
