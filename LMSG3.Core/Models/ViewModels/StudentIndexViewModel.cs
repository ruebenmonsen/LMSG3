using LMSG3.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Models.ViewModels
{
    public class StudentIndexViewModel
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string Id { get; set; }


        // My uploaded documents TODO: consolidte with more
        public ICollection<Document> Documents { get; set; }

        public ICollection<Activity> Activities { get; set; }

        public ICollection<Module> Modules { get; set; }

        public ICollection<Student> CourseStudents { get; set; }

        
        public CourseInfoViewModel CourseInfo { get; set; }

        public CurrentModuleViewModel CurrentModule { get; set; }
        
    }
}
