using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Models.ViewModels
{
    public class IndexCourseViewModel
    {
        public IEnumerable<CourseIndexViewModel> CoursesList { get; set; }
    }
}
