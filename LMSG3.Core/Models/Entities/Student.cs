using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Models.Entities
{
    public class Student
    {
        // FK
        public int CourseId { get; set; }

        // NAV
        public Course Course { get; set; }

    }
}
