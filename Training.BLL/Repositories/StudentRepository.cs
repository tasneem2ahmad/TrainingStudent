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
    public class StudentRepository : IGenericRepository<Student>, IStudentRepository
    {
        private readonly SchoolingContext context;

        public StudentRepository(SchoolingContext context)
        {
            this.context = context;
        }
        public async Task<int> Add(Student student)
        {
            context.AddAsync(student);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(Student student)
        {
            student.IsDeleted = true;
            await Update(student);
            return await context.SaveChangesAsync();
        }

        public async Task<Student> Get(int? id)
        {
            return await context.Students.FindAsync(id);
        }

        public async Task<IEnumerable<Student>> GetAll() => await context.Students.Where(c => !c.IsDeleted).ToListAsync();
        

        public async Task<int> Update(Student student)
        {
            context.Update(student);
            return await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Student>> SearchStudent(string value)
        => await context.Students.Where(c => c.Name.Contains(value)).ToListAsync();
    }
}
