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
    public class CourseStudentRepository : IGenericRepository<CourseStudent>, ICourseStudentRepository
    {
        private readonly SchoolingContext context;

        public CourseStudentRepository(SchoolingContext context)
        {
            this.context = context;
        }
        public async Task<int> Add(CourseStudent courseStudent)
        {
            context.AddAsync(courseStudent);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(CourseStudent courseStudent)
        {
            courseStudent.IsDeleted = true;
            await Update(courseStudent);
            return await context.SaveChangesAsync();
        }

        public async Task<CourseStudent> Get(int? id)
        {
            return await context.CourseStudents.FindAsync(id);
        }

        public async Task<IEnumerable<CourseStudent>> GetAll() => await context.CourseStudents.Where(c => !c.IsDeleted).ToListAsync();



        public async Task<IEnumerable<CourseStudent>> SearchCourseStudent(string value)
        => await context.CourseStudents.Where(c => c.Student.Name.Contains(value)).ToListAsync();

        public async Task<int> Update(CourseStudent courseStudent)
        {
            context.Update(courseStudent);
            return await context.SaveChangesAsync();
        }
    }
}
