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
    public class DepartmentRepository : IGenericRepository<Department>,IDepartmentRepository
    {
        private readonly SchoolingContext context;
        public DepartmentRepository(SchoolingContext context) {
            this.context = context;
        }
        public async Task<int> Add(Department departmet)
        {
            context.AddAsync(departmet);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(Department departmet)
        {
            context.Remove(departmet);
            return await context.SaveChangesAsync();
        }

        public async Task< Department> Get(int? id)
        {
            return await context.Departments.FindAsync(id);
        }

        public async Task< IEnumerable<Department>> GetAll() =>await context.Departments.ToListAsync();
        

        public async Task<int> Update(Department departmet)
        {
            context.Update(departmet);
            return await context.SaveChangesAsync();
        }
    }
}
