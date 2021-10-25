using LMSG3.Core.Models.Entities;
using LMSG3.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Data.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            var course = await context.Courses.Take(10).ToListAsync();
            return course;
        }

        public async Task<IEnumerable<Course>> GetAllCourses(bool includemodules)
        {
            return includemodules ?
                await context.Courses.Include(c => c.Modules).ThenInclude(m=>m.Activities).Take(10).ToListAsync() : await context.Courses.ToListAsync();
        }

        public async Task<Course> GetCourse(int? id, bool includemodules)
        {
            var query = context.Courses.AsQueryable();
            if (includemodules)
                query = query.Include(c => c.Modules).Include(c => c.Documents).ThenInclude(c=>c.DocumentType);

            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await context.Courses.AnyAsync(e => e.Id == id);
        }
        
        public bool Any(int id)
        {
            return context.Courses.Any(e => e.Id == id);
        }

        public void Update(Task<Course> course)
        {
            context.Update(course);
        }

        public override void Remove(Course entity)
        {
            context.Remove(entity);
        }
    }
}