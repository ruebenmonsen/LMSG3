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
    public class CourseRepository2 : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository2(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            var course = await context.Courses.ToListAsync();
            return course;
        }

        public async Task<IEnumerable<Course>> GetAllCourses(bool includemodules)
        {
            return includemodules ?
                await context.Courses.Include(c => c.Modules).ToListAsync() : await context.Courses.ToListAsync();
        }

        public async Task<Course> GetCourse(int? id, bool includemodules)
        {
            var query = context.Courses.AsQueryable();
            if (includemodules)
                query = query.Include(c => c.Modules);

            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        public bool Any(int id)
        {
            return context.Courses.Any(e => e.Id == id);
        }
    }
}