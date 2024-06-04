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
    public class GenericRepositry <T>:IGenericRepository<T> where T:class
    {
        private readonly SchoolingContext context;
        public GenericRepositry(SchoolingContext context)
        {
            this.context = context;
        }
        public async Task<int> Add(T item)
        {
            context.Set<T>().AddAsync(item);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(T item)
        {
            context.Set<T>().Remove(item);
            return await context.SaveChangesAsync();
        }

        public async Task<T> Get(int? id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll() =>await context.Set<T>().ToListAsync();


        public async Task<int> Update(T item)
        {
            context.Set<T>().Update(item);
            return await context.SaveChangesAsync();
        }
    }
}
