using Microsoft.AspNetCore.Identity;
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
    public class CourseRepository : IGenericRepository<Course>, ICourseRepository
    {
        private readonly SchoolingContext context;

        public CourseRepository(SchoolingContext context)
        {
            this.context = context;
        }

        public async Task<int> Add(Course course)
        {
            context.AddAsync(course);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(Course course)
        {
            context.Remove(course);
            return await context.SaveChangesAsync();
        }

        public async Task<Course> Get(int? id)
        {
            return await context.Courses.FindAsync(id);
        }

        public async Task<IEnumerable<Course>> GetAll() => await context.Courses.ToListAsync();

        public async Task<IEnumerable<Course>> SearchCourse(string value)
        => await context.Courses.Where(c => c.Name.Contains(value)).ToListAsync();


        public async Task<int> Update(Course course)
        {
            context.Update(course);
            return await context.SaveChangesAsync();
        }
    }
}
