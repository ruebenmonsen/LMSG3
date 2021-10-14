using LMSG3.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetAllCourses();
        Task<IEnumerable<Course>> GetAllCourses(bool includemodules);
        Task<Course> GetCourse(int? id, bool includemodules);
        bool Any(int id);
       
    }
}
