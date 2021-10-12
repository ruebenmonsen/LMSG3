using LMSG3.Core.Models.Entities;
using LMSG3.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private ApplicationDbContext db;

        public CourseRepository(ApplicationDbContext db)
        {
            this.db = db;
        }


        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await db.Courses.ToListAsync();
        }


        public async Task<IEnumerable<Course>> GetAllCourses(bool includemodules)
        {
            return includemodules ?
                await db.Courses.Include(c => c.Modules).ToListAsync() : await db.Courses.ToListAsync();
        }
        public async Task<Course> GetCourse(int? id, bool includemodules)
        {
            var query = db.Courses.AsQueryable();
            if (includemodules)
                query = query.Include(c => c.Modules);

            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }


        public void Add(Course course)
        {
            db.Add(course);
        }


        public bool Any(int id)
        {
            return db.Courses.Any(e => e.Id == id);
        }


        public void Remove(Course course)
        {
            db.Courses.Remove(course);
        }

        public void Update(Course course)
        {
            db.Update(course);
        }

    }
}
