﻿using LMSG3.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCourses();
        Task<IEnumerable<Course>> GetAllCourses(bool includemodules);
        Task<Course> GetCourse(int? id, bool includemodules);
        Task<bool> AnyAsync(int id);
        void Add(Course course);
        bool Any(int id);
        void Update(Task<Course> course);
        void Remove(Course course);
        void Update(Course course);

       
    }
}
