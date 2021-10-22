using LMSG3.Core.Models.Entities;
using LMSG3.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Data.Repositories
{
    public class StudentRepository : IStudentRepository

    {
        private ApplicationDbContext context;

        public StudentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Student>> Getparticipants(int id)
        {
            var course = await context.Students.Where(s => s.CourseId == id).ToListAsync();
            return course;
        }
    }
}
